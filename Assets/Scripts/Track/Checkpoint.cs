using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CheckpointBackCollider _backCollider;
    [SerializeField] private CheckpointFrontCollider _frontCollider;
    [SerializeField] private Transform _respawnPoint;

    private GameObject _vehicleGoingThroughCheckpoint;
    private bool _playerRidingBack = false;
    private Vector3 _playerForwardOnCheckpoint;

    public bool VehicleExitedCorrectly { get; private set; }
    public List<Checkpoint> ReverseCheckpointOnEnter = new List<Checkpoint>();
    private bool hasReversed = false;

    private void OnTriggerExit(Collider other)
    {
        Transform car = other.transform;

        // When vehicle hits checkpoint
        if (car != null && car.CompareTag("Player"))
        {
            _vehicleGoingThroughCheckpoint = other.gameObject;
            _playerForwardOnCheckpoint = other.transform.forward;
            CheckVehicleExit();
            RaceManager.Instance.CmdVehicleThroughCheckPoint(RaceManager.Instance.GetCheckpointIndex(this), _vehicleGoingThroughCheckpoint, VehicleExitedCorrectly);
            _vehicleGoingThroughCheckpoint.GetComponentInChildren<VehiclePositionTrigger>()?.SetTransformSize(transform.rotation, transform.localScale);
            _vehicleGoingThroughCheckpoint.GetComponentInChildren<VehiclePositionTrigger>()?.SetCheckpointTransform(transform);
            if(VehicleExitedCorrectly && !hasReversed)
            {
                hasReversed = true;
                foreach (Checkpoint checkpoint in ReverseCheckpointOnEnter) checkpoint.ReverseOrientation();
            }
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

    private void ReverseOrientation()
    {
        _playerRidingBack = !_playerRidingBack;
    }

    public Vector3 RespawnPoint => _respawnPoint ? _respawnPoint.transform.position : GetComponent<Collider>().bounds.center;
    public Vector3 RespawnOrientation => _playerForwardOnCheckpoint;
}
