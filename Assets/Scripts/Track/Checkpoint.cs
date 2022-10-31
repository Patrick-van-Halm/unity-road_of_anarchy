using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CheckpointBackCollider _backCollider;
    [SerializeField] private CheckpointFrontCollider _frontCollider;

    private GameObject _vehicleGoingThroughCheckpoint;
    private bool _playerRidingBack = false;

    public bool VehicleExitedCorrectly { get; private set; }

    private void OnTriggerExit(Collider other)
    {
        Transform car = other.transform;

        // When vehicle hits checkpoint
        if (car != null && car.CompareTag("Player"))
        {
            _vehicleGoingThroughCheckpoint = other.gameObject;
            CheckVehicleExit();
            RaceManager.Instance.CmdVehicleThroughCheckPoint(RaceManager.Instance.GetCheckpointIndex(this), _vehicleGoingThroughCheckpoint);
            _vehicleGoingThroughCheckpoint.GetComponentInChildren<VehiclePositionTrigger>()?.SetTransformSize(transform.rotation, transform.localScale);
            _vehicleGoingThroughCheckpoint.GetComponentInChildren<VehiclePositionTrigger>()?.SetCheckpointTransform(transform);
        }
    }

    private void CheckVehicleExit()
    {
        if (!_playerRidingBack)
        {
            if (!_backCollider.Entered && _frontCollider.Entered) VehicleExitedCorrectly = true;
            if (_backCollider.Entered && !_frontCollider.Entered) VehicleExitedCorrectly = false;
        }
        else
        {
            if (!_backCollider.Entered && _frontCollider.Entered) VehicleExitedCorrectly = false;
            if (_backCollider.Entered && !_frontCollider.Entered) VehicleExitedCorrectly = true;
        }
    }
}
