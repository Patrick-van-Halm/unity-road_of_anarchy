using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;

    private Rigidbody rb;

    private bool _checkpoint2 = false;

    private void Start()
    {
        rb = _parentObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_checkpoint2 && other.gameObject.tag == "Terrain")
        {
            _parentObject.transform.position = SpawnManager.Instance.respawn1.position;
            _parentObject.transform.rotation = SpawnManager.Instance.respawn1.rotation;
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
        }
        else if (other.gameObject.tag == "Terrain")
        {
            _parentObject.transform.position = SpawnManager.Instance.respawn2.position;
            _parentObject.transform.rotation = SpawnManager.Instance.respawn2.rotation;
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
        }

        if (other.gameObject.tag == "Checkpoint") _checkpoint2 = true;
    }

    public bool CanFinish()
    {
        return _checkpoint2;
    }
}
