using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WreckedCar : NetworkBehaviour
{
    [SerializeField] private GameObject _wreckedBuggy;

    public GameObject WreckedBuggy { get { return _wreckedBuggy; } }
    private Transform _position;

    public UnityEvent<string> OnKillMessage;
    public UnityEvent<string> OnLastTeamStanding;

    private AttributeComponent _vehicleAttributes;

    //private List<Team> _eliminatedTeams;
    //[SyncVar(hook = nameof(LastTeamStanding))]
    //public List<Team> EliminatedTeams;
    //public List<Team> EliminatedTeams
    //{
    //    get { return _eliminatedTeams; }
    //    set
    //    {
    //        if (value.Count == SpawnManager.Instance.AllTeams.Count - 1)
    //        {
    //            OnLastTeamStanding?.Invoke("You have killed the last team standing. You win!");
    //        }
    //    }
    //}

    //public void LastTeamStanding(List<Team> oldValue, List<Team> newValue)
    //{
    //    if (newValue.Count == SpawnManager.Instance.AllTeams.Count - 1)
    //    OnLastTeamStanding?.Invoke("Your team has killed the last remaining team. You win!");
    //}

    private void Awake()
    {
        _vehicleAttributes = GetComponent<AttributeComponent>();
    }

    private void Start()
    {
        if (!isServer) return;
        _vehicleAttributes.OnHealthChanged.AddListener(PlaceWreckedCarWhenHealthZero);
    }

    public void PlaceWreckedCarWhenHealthZero(float health)
    {
        if (health > 0) return;

        _position = GetComponent<Transform>();

        if (NetworkServer.active)
        {
            _vehicleAttributes.LastDamagedBy.Gunner.TargetOnTeamKill(_vehicleAttributes.LastDamagedBy.GunnerIdentity.connectionToClient);
            _vehicleAttributes.LastDamagedBy.Vehicle.TargetOnTeamKill(_vehicleAttributes.LastDamagedBy.DriverIdentity.connectionToClient);

            // Spawn as spectators
            Player player = GetComponent<Player>();
            FindObjectOfType<SpawnManager>()?.SpawnSpectators(player.Team);

            // Send elimination rpc to team
            if(player.Team.DriverSpectator) player.Team.DriverSpectator.wasEliminated = true;
            if(player.Team.GunnerSpectator) player.Team.GunnerSpectator.wasEliminated = true;

            SpawnManager.Instance.TeamEliminated(player.Team);
        }
        else OnKillMessage?.Invoke("You killed a team");      

        _wreckedBuggy = Instantiate(_wreckedBuggy, _position.position, _position.rotation);
        if (NetworkServer.active)
        {
            NetworkServer.Spawn(_wreckedBuggy);
        }
    }
}
