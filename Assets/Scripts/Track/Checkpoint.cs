using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private RaceManager _raceManager;

    private void OnTriggerEnter(Collider other)
    {
        // When vehicle hits checkpoint
        if(other.TryGetComponent(out Car car))
        {
            _raceManager.VehicleThroughCheckPoint(this, other.transform);
        }
    }

    /// <summary>
    /// Sets a reference to the RaceManager script
    /// </summary>
    /// <param name="raceManagerScript"></param>
    public void SetRaceManagerScript(RaceManager raceManagerScript)
    {
        _raceManager = raceManagerScript;
    }
}
