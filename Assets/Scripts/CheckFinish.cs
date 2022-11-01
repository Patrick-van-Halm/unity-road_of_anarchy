using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinish : MonoBehaviour
{
    [SerializeField] CarRespawn _carRespawn;

    private bool _canFinish = false;

    private void OnTriggerEnter(Collider other)
    {
        _canFinish = _carRespawn.CanFinish();
        if (_canFinish && other.gameObject.tag == "Finish") print("Hey great job. You past the border.");
        else if (other.gameObject.tag == "Finish") print("No cheating!");
    }
}
