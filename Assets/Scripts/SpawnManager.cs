using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject _carObject;
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private List<Transform> _spawns = new List<Transform>();
    [SerializeField] private PlayerHUDComponent _hudComponent;
    private GameObject _lastCarObject;
    private GameObject _lastGunObject;

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
        if (_lastCarObject == null || _lastGunObject != null)
        {
            GameObject newPlayerObj = Instantiate(_carObject, _spawns.Random().position, Quaternion.identity);
            NetworkServer.ReplacePlayerForConnection(conn, newPlayerObj);
            _lastCarObject = newPlayerObj;
            _lastGunObject = null;
        }
        else
        {
            GameObject newPlayerObj = Instantiate(_gunObject);
            newPlayerObj.name = "Gunner";

            GlueTo glueTo = newPlayerObj.GetComponent<GlueTo>();
            glueTo.Target = _lastCarObject.transform;
            glueTo.LocalPosition = new Vector3(0, 2, 0);

            NetworkServer.ReplacePlayerForConnection(conn, newPlayerObj);
            _lastGunObject = newPlayerObj;
        }
        Destroy(currentPlayerObj, .1f);
        RpcLinkToCar(conn, _lastCarObject);
    }

    [TargetRpc]
    private void RpcLinkToCar(NetworkConnection conn, GameObject car)
    {
        car.GetComponent<AttributeComponent>().OnHealthChanged.AddListener(_hudComponent.OnHealthChanged);
    }
}
