using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject _carObject;
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _spectatorObject;
    [SerializeField] private Vector3 _gunObjectOffset;
    [SerializeField] private List<Transform> _spawns = new List<Transform>();
    [SerializeField] private PlayerHUDComponent _hudComponent;
    private GameObject _currentCarObject;
    private GameObject _currentGunObject;
    private Team _team;
    private bool _isGunnerAssigned;

    private void Awake()
    {
        if (_spawns.Count == 0) enabled = false;
        if (_carObject == null) enabled = false;
        if (_gunObject == null) enabled = false;

        // Run only on server
        if (!NetworkServer.active) return;

        // Listen to the add player (is called every scene switch)
        GameNetworkManager.Instance.OnServerAddPlayerToClient.AddListener(ReplacePlayerWithPrefab);
    }

    private void ReplacePlayerWithPrefab(NetworkConnectionToClient conn)
    {
        GameObject currentPlayerObj = conn.identity.gameObject;
        if (_isGunnerAssigned || _currentCarObject == null || _currentGunObject == null)
        {
            _team = new Team();

            _isGunnerAssigned = false;
            _currentCarObject = Instantiate(_carObject, _spawns.Random().position, Quaternion.identity);
            _team.DriverIdentity = _currentCarObject.GetComponent<NetworkIdentity>();
            NetworkServer.ReplacePlayerForConnection(conn, _currentCarObject);

            _currentGunObject = Instantiate(_gunObject);
            NetworkServer.Spawn(_currentGunObject);
            _team.GunnerIdentity = _currentGunObject.GetComponent<NetworkIdentity>();

            GlueTo glueTo = _currentGunObject.GetComponent<GlueTo>();
            glueTo.Target = _currentCarObject.transform;
            glueTo.LocalPosition = _gunObjectOffset;

            _team.Vehicle.Team = _team;
            _team.Gunner.Team = _team;
        }
        else
        {
            NetworkServer.ReplacePlayerForConnection(conn, _currentGunObject);
            _isGunnerAssigned = true;
        }

        Destroy(currentPlayerObj, .1f);
        RpcLinkToCar(conn, _currentCarObject, _currentGunObject);
    }

    [TargetRpc]
    private void RpcLinkToCar(NetworkConnection conn, GameObject car, GameObject gunner)
    {
        car.GetComponent<AttributeComponent>().OnHealthChanged.AddListener(_hudComponent.OnHealthChanged);
        car.GetComponent<BoxCollider>().enabled = false;
    }

    [ServerCallback]
    public void SpawnSpectators(Team team)
    {
        GameObject oldGunner = team.GunnerIdentity.gameObject;
        GameObject oldDriver = team.DriverIdentity.gameObject;

        NetworkConnectionToClient conn = team.GunnerIdentity.connectionToClient;
        NetworkServer.ReplacePlayerForConnection(conn, Instantiate(_spectatorObject));
        team.GunnerIdentity = conn.identity;

        conn = team.DriverIdentity.connectionToClient;
        NetworkServer.ReplacePlayerForConnection(conn, Instantiate(_spectatorObject));
        team.DriverIdentity = conn.identity;

        Destroy(oldDriver, 0.1f);
        Destroy(oldGunner, 0.1f);
    }
}
