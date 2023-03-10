using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : NetworkBehaviour
{
    [Header("Spawnable Objects")]
    [SerializeField] private GameObject _carObject;
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _spectatorObject;

    [Header("Spawnable Settings")]
    [SerializeField] private Vector3 _gunObjectOffset;

    [Header("Spawn settings")]
    [SerializeField] private List<Transform> _spawns = new List<Transform>();
    [SerializeField] private PlayerHUDComponent _hudComponent;
    private List<Transform> _usedSpawns = new List<Transform>();
    private GameObject _currentCarObject;
    private GameObject _currentGunObject;

    private readonly List<Team> _eliminatedTeams = new List<Team>();

    public UnityEvent<string> OnLastTeamStanding;

    public static SpawnManager Instance { get; private set; }

    private readonly List<Team> _allTeams = new List<Team>();

    private readonly List<Team> _eliminatedTeams = new List<Team>();

    public UnityEvent<string> OnLastTeamStanding;

    public static SpawnManager Instance { get; private set; }

    private void Awake()
    {
        if (_spawns.Count == 0) enabled = false;
        if (_carObject == null) enabled = false;
        if (_gunObject == null) enabled = false;

        if (Instance == null) Instance = this;
        else Destroy(this);

        // Run only on server
        if (!NetworkServer.active) return;

        // Listen to the add player (is called every scene switch)
        GameNetworkManager.Instance.OnServerAddPlayerToClient.AddListener(ReplacePlayerWithPrefab);
    }

    public void TeamEliminated(Team team)
    {
        _eliminatedTeams.Add(team);
        LastTeamStanding();
    }

    private void LastTeamStanding()
    {
        List<Team> teamsAlive = TeamManager.Instance.Teams.Where(t => !_eliminatedTeams.Any(t2 => t2 == t)).ToList();
        if (teamsAlive.Count == 1)
        {
            SpawnSpectators(teamsAlive[0]);
            TargetOnLastTeamStanding(teamsAlive[0].GunnerIdentity.connectionToClient);
            TargetOnLastTeamStanding(teamsAlive[0].DriverIdentity.connectionToClient);
        }
    }

    [TargetRpc]
    private void TargetOnLastTeamStanding(NetworkConnection target)
    {
        OnLastTeamStanding?.Invoke("Your team has killed the last remaining team. You win!");
    }

    [Server]
    private void ReplacePlayerWithPrefab(NetworkConnectionToClient conn)
    {
        Team _team = TeamManager.Instance?.Teams.FirstOrDefault(t => t.DriverClientId == conn.connectionId || t.GunnerClientId == conn.connectionId);
        GameObject currentPlayerObj = conn.identity.gameObject;

        if(_team.DriverIdentity == null && _team.GunnerIdentity == null)
        {
            Transform spawn = _spawns.Where(s => !_usedSpawns.Contains(s)).Random();
            _usedSpawns.Add(spawn);

            _currentCarObject = Instantiate(_carObject, spawn.position, Quaternion.identity);
            _team.DriverIdentity = _currentCarObject.GetComponent<NetworkIdentity>();

            _currentGunObject = Instantiate(_gunObject);
            _team.GunnerIdentity = _currentGunObject.GetComponent<NetworkIdentity>();

            GlueTo glueTo = _currentGunObject.GetComponent<GlueTo>();
            glueTo.Target = _currentCarObject.transform;
            glueTo.LocalPosition = _gunObjectOffset;

            NetworkServer.Spawn(_currentCarObject);
            NetworkServer.Spawn(_currentGunObject);

            RaceManager.Instance?.AddVehicleToList(_currentCarObject);
        }

        if(_team.DriverClientId == conn.connectionId)
        {
            NetworkServer.ReplacePlayerForConnection(conn, _team.DriverIdentity.gameObject);
            RpcHideGunnerUI(conn);
        }
        else if(_team.GunnerClientId == conn.connectionId)
        {
            NetworkServer.ReplacePlayerForConnection(conn, _team.GunnerIdentity.gameObject);
            RpcDisableCarCollider(conn, _team.DriverIdentity.gameObject);
        }

        Destroy(currentPlayerObj, .1f);
        RpcLinkToCar(conn, _team.DriverIdentity.gameObject, _team.GunnerIdentity.gameObject);

        if(TeamManager.Instance && TeamManager.Instance.Teams.All(t => t.DriverIdentity && t.DriverIdentity.connectionToClient != null && t.GunnerIdentity && t.GunnerIdentity.connectionToClient != null))
        {
            // Last member has been assigned
            foreach(Team team in TeamManager.Instance.Teams)
            {
                TargetCreateTeamsWorldspacePlayerNameUI(team.DriverIdentity.connectionToClient, TeamManager.Instance.Teams.ToList());
                TargetCreateTeamsWorldspacePlayerNameUI(team.GunnerIdentity.connectionToClient, TeamManager.Instance.Teams.ToList());

                team.DriverPlayer.Team = team;
                team.GunnerPlayer.Team = team;
            }
        }
    }

    [TargetRpc]
    private void TargetCreateTeamsWorldspacePlayerNameUI(NetworkConnection target, List<Team> teams)
    {
        foreach(Team team in teams)
            _hudComponent.CreateTeamPlayerNamesUI(team);
    }

    [TargetRpc]
    private void TargetCreateTeamWorldspacePlayerNameUI(NetworkConnection target, Team team)
    {
        _hudComponent.CreateTeamPlayerNamesUI(team);
    }

    [TargetRpc]
    private void RpcHideGunnerUI(NetworkConnection conn)
    {
        _hudComponent.HideGunnerUI();
    }

    [TargetRpc]
    private void RpcHideGunnerUI(NetworkConnection conn)
    {
        _hudComponent.HideGunnerUI();
    }

    [TargetRpc]
    private void RpcLinkToCar(NetworkConnection conn, GameObject car, GameObject gunner)
    {
        OnLastTeamStanding.AddListener(_hudComponent.ShowWinUI);
        RaceManager.Instance.FinishRace.AddListener(_hudComponent.ShowWinUI);
        car.GetComponent<AttributeComponent>().OnHealthChanged.AddListener(_hudComponent.OnHealthChanged);
        gunner.GetComponent<WeaponManager>().OnHeatChanged.AddListener(_hudComponent.OnHeatChanged);
        _hudComponent.SetMaxHeat(gunner.GetComponent<WeaponManager>().Weapon.HeatMaxValue);
        car.GetComponent<Vehicle>().OnInWater.AddListener(gunner.GetComponent<WeaponManager>().WaterCooldown);
        car.GetComponentInChildren<NewKartScript>().PostFX = FindObjectOfType<PostFXScript>();
        car.tag = "Player";

        gunner.GetComponent<WeaponManager>().SetSlowdownEffectHandler(car.GetComponent<SlowdownEffectHandler>());

        _hudComponent.SetPlayerNames(car.GetComponent<Player>(), gunner.GetComponent<Player>());
        _hudComponent.CreateMinimap(car);
    }

    [TargetRpc]
    private void RpcDisableCarCollider(NetworkConnection conn, GameObject car)
    {
        car.GetComponent<BoxCollider>().enabled = false;
    }

    [ServerCallback]
    public void SpawnSpectators(Team team)
    {
        GameObject oldGunner = team.GunnerIdentity.gameObject;
        GameObject oldDriver = team.DriverIdentity.gameObject;

        NetworkConnectionToClient conn = team.GunnerIdentity.connectionToClient;
        if(conn != null)
        {
            NetworkServer.ReplacePlayerForConnection(conn, Instantiate(_spectatorObject));
            team.GunnerIdentity = conn.identity;
        }

        conn = team.DriverIdentity.connectionToClient;
        if (conn != null)
        {
            NetworkServer.ReplacePlayerForConnection(conn, Instantiate(_spectatorObject));
            team.DriverIdentity = conn.identity;
        }

        Destroy(oldDriver, 0.1f);
        Destroy(oldGunner, 0.1f);
    }
}
