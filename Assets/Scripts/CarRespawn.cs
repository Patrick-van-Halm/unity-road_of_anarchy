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

            Vector3 centerOfCheckpoint = lastCheckpoint.GetComponent<Collider>().bounds.center;

            _parentObject.transform.position = centerOfCheckpoint;
            _parentObject.transform.rotation = lastCheckpoint.transform.rotation;
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
        }
    }
}
