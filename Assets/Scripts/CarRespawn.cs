using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;

    private Rigidbody rb;

    private void Start()
    {
        rb = _parentObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            Checkpoint lastCheckpoint = RaceManager.Instance?.LastCheckpoint();
            if (lastCheckpoint == null) return;

            _parentObject.transform.position = lastCheckpoint.RespawnPoint;
            _parentObject.transform.forward = lastCheckpoint.RespawnOrientation;
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
        }
    }
}
