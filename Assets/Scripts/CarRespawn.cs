using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;
    [SerializeField] private LayerMask _environmentLayers;

    private Rigidbody rb;

    private void Start()
    {
        rb = _parentObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Physics.Raycast(_parentObject.transform.position, -_parentObject.transform.up, out RaycastHit hit, 2f, _environmentLayers))
        {
            if (hit.collider.CompareTag("Terrain")) RespawnCar();
        }
    }

    private void RespawnCar()
    {
        Checkpoint lastCheckpoint = RaceManager.Instance?.LastCheckpoint();
        if (lastCheckpoint == null) return;

        _parentObject.transform.position = lastCheckpoint.RespawnPoint;
        _parentObject.transform.forward = lastCheckpoint.RespawnOrientation;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
    }
}
