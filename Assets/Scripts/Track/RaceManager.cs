using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static RaceManager;

public class RaceManager : NetworkBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private int _numberOfLaps;

    [Header("Checkpoints")]
    [SerializeField] private List<Checkpoint> _checkpointsList;

    public static RaceManager Instance { get; private set; }
    
    public int NumberOfLaps => _numberOfLaps;
    private SyncList<VehiclePosition> _vehiclePositionList = new SyncList<VehiclePosition>();
    private List<DynamicOvertakeTrigger> _dynamicOvertakeTriggerList;
    //private List<GameObject> CheckpointsDrivenThrough = new List<GameObject>();

    public class VehiclePosition
    {
        public GameObject Vehicle;
        public List<GameObject> CheckpointsDrivenThrough = new List<GameObject>();
        public int CurrentCheckpoint;
        public int CurrentLap;
    }

    private class DynamicOvertakeTrigger
    {
        public GameObject CurrentVehicle;
        public GameObject CollisionVehicle;
        public bool IsFrontTrigger;
    }

    [Header("Events")]
    public UnityEvent WrongCheckpoint = new UnityEvent();
    public UnityEvent CorrectCheckpoint = new UnityEvent();
    public UnityEvent<int> IncreaseLap = new UnityEvent<int>();
    public UnityEvent<int> OnPositionUpdate = new UnityEvent<int>();
    public UnityEvent<int, string> FinishRace = new UnityEvent<int, string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        // Initialization
        _dynamicOvertakeTriggerList = new List<DynamicOvertakeTrigger>();
    }

    private void Start()
    {
        FindObjectOfType<RaceManagerUI>().RaceManagerReady();
    }

    public void AddVehicleToList(GameObject vehicle)
    {
        // Set starting values 
        _vehiclePositionList.Add(new VehiclePosition { Vehicle = vehicle, CurrentLap = 0, CurrentCheckpoint = 0 });
    }

    public int GetCheckpointIndex(Checkpoint checkpoint)
    {
        return _checkpointsList.IndexOf(checkpoint);
    }

    public Checkpoint LastCheckpoint()
    {
        Vehicle myVehicle = NetworkClient.connection.identity.GetComponent<Vehicle>();
        if(myVehicle == null) return null;

        VehiclePosition pos = _vehiclePositionList.Find(v => v.Vehicle == myVehicle.gameObject);
        //GameObject lastCheckpointObj = pos.CheckpointsDrivenThrough[pos.CheckpointsDrivenThrough.Count - 1];

        //return lastCheckpointObj.GetComponent<Checkpoint>();
        return _checkpointsList[pos.CurrentCheckpoint - 1];
    }

    [Command(requiresAuthority = false)]
    public void CmdVehicleThroughCheckPoint(int checkpointIndex, GameObject vehicle, bool exitedCorrectly)
    {
        VehicleThroughCheckPoint(_checkpointsList[checkpointIndex], vehicle, exitedCorrectly);
    }

    /// <summary>
    /// This function gets called from a checkpoint gameobject when a vehicle collides with it. 
    /// This method is responsible for checking if the vehicle went through the right checkpoint. 
    /// Futhermore, it will trigger another method (CheckRacePosition) which will update the vehicle positions.
    /// </summary>
    /// <param name="checkpoint"></param>
    /// <param name="vehicle"></param>
    private void VehicleThroughCheckPoint(Checkpoint checkpoint, GameObject vehicle, bool exitedCorrectly)
    {
        // Get right index of current vehicle in sorted list
        int vehicleIndex = 0;
        for (int i = 0; i < _vehiclePositionList.Count; i++)
        {
            if(vehicle == _vehiclePositionList[i].Vehicle)
            {
                vehicleIndex = i;
                break;
            }
        }

        // Check if vehicle is on the right checkpoint
        if (_checkpointsList[_vehiclePositionList[vehicleIndex].CurrentCheckpoint] == checkpoint)
        {
            AddCheckpointToVehiclePositionList(vehicleIndex, checkpoint);

            // Increase checkpoint counter for that vehicle
            _vehiclePositionList[vehicleIndex].CurrentCheckpoint++;

            // Correct checkpoint event
            TargetCorrectCheckpoint(_vehiclePositionList[vehicleIndex].Vehicle.GetComponent<NetworkIdentity>().connectionToClient);

            // When lap is completed
            if (_checkpointsList.Count == _vehiclePositionList[vehicleIndex].CurrentCheckpoint && _checkpointsList.Count == _vehiclePositionList[vehicleIndex].CheckpointsDrivenThrough.Count - _vehiclePositionList[vehicleIndex].CurrentLap * _checkpointsList.Count)
            {
                _vehiclePositionList[vehicleIndex].CurrentLap++;                                        // Increase lap for vehicle
                _vehiclePositionList[vehicleIndex].CurrentCheckpoint = 0;                               // Reset checkpoint

                Team team = _vehiclePositionList[vehicleIndex].Vehicle.GetComponent<Player>().Team;
                if (_vehiclePositionList[vehicleIndex].CurrentLap == _numberOfLaps)
                {
                    // Spawn spectators
                    SpawnManager.Instance?.SpawnSpectators(team);

                    // Finished race
                    if (team.DriverIdentity.connectionToClient != null) TargetFinishRace(team.DriverIdentity.connectionToClient, vehicleIndex + 1);
                    if (team.GunnerIdentity.connectionToClient != null) TargetFinishRace(team.GunnerIdentity.connectionToClient, vehicleIndex + 1);
                }
                else
                {
                    // Increaselap event
                    if (team.DriverIdentity.connectionToClient != null) TargetIncreaseLap(team.DriverIdentity.connectionToClient, _vehiclePositionList[vehicleIndex].CurrentLap);
                    if (team.GunnerIdentity.connectionToClient != null) TargetIncreaseLap(team.GunnerIdentity.connectionToClient, _vehiclePositionList[vehicleIndex].CurrentLap);
                }
            }
        }
        else if(_checkpointsList[_vehiclePositionList[vehicleIndex].CurrentCheckpoint] != checkpoint && _checkpointsList[_vehiclePositionList[vehicleIndex].CurrentCheckpoint - 1] != checkpoint)
        {
            // Wrong checkpoint
            TargetWrongCheckpoint(_vehiclePositionList[vehicleIndex].Vehicle.GetComponent<NetworkIdentity>().connectionToClient);

            // Change the current checkpoint to checkpoint that is next
            //_vehiclePositionList[vehicleIndex].CurrentCheckpoint = _checkpointsList.IndexOf(checkpoint) + 1;

            // Update the list to till the current driven checkpoint
            //_vehiclePositionList[vehicleIndex].CheckpointsDrivenThrough = _checkpointsList.GetRange(0, _checkpointsList.IndexOf(checkpoint)).Select(c => c.gameObject).ToList();
        }

        // Set the position of the vehicle
        CheckRacePosition();
    }

    /// <summary>
    /// Adds checkpoint to list of vehicle if the checkpoint isn't already in the list.
    /// </summary>
    /// <param name="vehicleListIndex"></param>
    /// <param name="checkpoint"></param>
    private void AddCheckpointToVehiclePositionList(int vehicleListIndex, Checkpoint checkpoint)
    {
        //List<Checkpoint> _currentCheckpointList = _vehiclePositionList[vehicleListIndex].CheckpointsDrivenThrough.Select(x => x.GetComponent<Checkpoint>()).ToList();
        //bool _isNewCheckpoint = true;
        //int _checkpointListLength = _vehiclePositionList[vehicleListIndex].CheckpointsDrivenThrough.Count;
        //for (int i = 0; i < _checkpointListLength; i++)
        //{
        //    if (_currentCheckpointList[i] == checkpoint)
        //    {
        //        _isNewCheckpoint = false;
        //        break;
        //    }
        //}

        //if (_isNewCheckpoint) _vehiclePositionList[vehicleListIndex].CheckpointsDrivenThrough.Add(checkpoint.gameObject);
        _vehiclePositionList[vehicleListIndex].CheckpointsDrivenThrough.Add(checkpoint.gameObject);
    }

    [TargetRpc]
    private void TargetWrongCheckpoint(NetworkConnection target)
    {
        WrongCheckpoint?.Invoke();
    }

    [TargetRpc]
    private void TargetIncreaseLap(NetworkConnection target, int currentLap)
    {
        IncreaseLap?.Invoke(currentLap);
    }

    [TargetRpc]
    private void TargetCorrectCheckpoint(NetworkConnection target)
    {
        CorrectCheckpoint?.Invoke();
    }

    [Command(requiresAuthority = false)]
    public void CmdVehicleThroughPositionCollider(GameObject currentVehicle, GameObject collisionVehicle, bool isFrontTrigger, bool hasEnteredTrigger)
    {
        VehicleThroughPositionCollider(currentVehicle, collisionVehicle, isFrontTrigger, hasEnteredTrigger);
    }

    /// <summary>
    /// This function is responsible for overtaking between the vehicles itself. It will be called from a trigger which is attached to the vehicles 
    /// </summary>
    /// <param name="currentVehicleTransform"></param>
    /// <param name="collisionVehicleTransform"></param>
    /// <param name="isFrontTrigger"></param>
    /// <param name="hasEnteredTrigger"></param>
    private void VehicleThroughPositionCollider(GameObject currentVehicle, GameObject collisionVehicle, bool isFrontTrigger, bool hasEnteredTrigger)
    {
        if(hasEnteredTrigger)
        {
            _dynamicOvertakeTriggerList.Add(new DynamicOvertakeTrigger() { CurrentVehicle = currentVehicle, CollisionVehicle = collisionVehicle, IsFrontTrigger = isFrontTrigger});
        }
        else
        {
            // Get all items from the list where currentvehicle collides with collisionvehicle
            List<DynamicOvertakeTrigger> tempList = new List<DynamicOvertakeTrigger>();
            for (int i = 0; i < _dynamicOvertakeTriggerList.Count; i++)
            {
                // Only store the currentvehicle and collisionvehicles
                if(_dynamicOvertakeTriggerList[i].CurrentVehicle == currentVehicle && _dynamicOvertakeTriggerList[i].CollisionVehicle == collisionVehicle ||
                   _dynamicOvertakeTriggerList[i].CurrentVehicle == collisionVehicle && _dynamicOvertakeTriggerList[i].CollisionVehicle == currentVehicle)
                {
                    tempList.Add(_dynamicOvertakeTriggerList[i]);
                }
            }

            // Remove duplicates
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                if (i >= tempList.Count) continue;
                DynamicOvertakeTrigger overtakeTrigger = tempList[i];
                tempList.RemoveAll(o => o.CollisionVehicle == overtakeTrigger.CollisionVehicle && o.CurrentVehicle == overtakeTrigger.CurrentVehicle && o.IsFrontTrigger == overtakeTrigger.IsFrontTrigger);
                tempList.Add(overtakeTrigger);
            }

            // Check if all colliders have been hit on the vehicle
            int isFrontCounter = 0;
            int isBackCounter = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                if(tempList[i].IsFrontTrigger)
                {
                    isFrontCounter++;
                }
                else
                {
                    isBackCounter++;
                }
            }

            // Vehicle has overtaken
            VehiclePosition currentVehiclePosition = _vehiclePositionList.Find(p => p.Vehicle == currentVehicle);
            VehiclePosition collisionVehiclePosition = _vehiclePositionList.Find(p => p.Vehicle == collisionVehicle);
            if (isFrontCounter + isBackCounter == 4 && currentVehiclePosition.CurrentLap == collisionVehiclePosition.CurrentLap)
            {
                SwapVehiclePositions(currentVehiclePosition, collisionVehiclePosition);

                // Find all vehicles which need to be removed
                List<int> removeVehicleIndexList = new List<int>();
                for (int i = 0; i < _dynamicOvertakeTriggerList.Count; i++)
                {
                    if (_dynamicOvertakeTriggerList[i].CurrentVehicle == currentVehicle && _dynamicOvertakeTriggerList[i].CollisionVehicle == collisionVehicle ||
                        _dynamicOvertakeTriggerList[i].CurrentVehicle == collisionVehicle && _dynamicOvertakeTriggerList[i].CollisionVehicle == currentVehicle)
                    {
                        removeVehicleIndexList.Add(i);
                    }
                }

                // Remove all vehicles
                for (int indexToRemove = removeVehicleIndexList.Count - 1; indexToRemove >= 0; indexToRemove--)
                {
                    _dynamicOvertakeTriggerList.RemoveAt(removeVehicleIndexList[indexToRemove]);
                }
            }
        }

        foreach (VehiclePosition vehiclePosition in _vehiclePositionList)
        {
            if (vehiclePosition.CurrentLap == _numberOfLaps) continue;
            Team team = vehiclePosition.Vehicle.GetComponent<Player>().Team;

            //TargetPositionUpdate(vehiclePosition.Vehicle.GetComponent<NetworkIdentity>().connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
            if (team.DriverIdentity.connectionToClient != null) TargetPositionUpdate(team.DriverIdentity.connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
            if (team.GunnerIdentity.connectionToClient != null) TargetPositionUpdate(team.GunnerIdentity.connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
        }
    }

    private void SwapVehiclePositions(VehiclePosition currentVehicle, VehiclePosition collisionVehicle)
    {
        //Swap vehicle positions around in list
        _vehiclePositionList[_vehiclePositionList.IndexOf(currentVehicle)] = collisionVehicle;
        _vehiclePositionList[_vehiclePositionList.IndexOf(collisionVehicle)] = currentVehicle;
    }

    private void CheckRacePosition()
    {
        // Sort based on lap
        IEnumerable<VehiclePosition> _temp = _vehiclePositionList.ToArray().OrderBy(x => -x.CurrentLap);
        _vehiclePositionList.Clear();
        _vehiclePositionList.AddRange(_temp);

        // Sort based on checkpoints when vehicles are on the same lap
        for (int lap = 0; lap < _numberOfLaps; lap++)
        {
            List<VehiclePosition> tempList = new List<VehiclePosition>();
            List<int> tempIndex = new List<int>(); 

            // Get all vehicles on the same lap
            int vehiclePositionListSize = _vehiclePositionList.Count;
            for (int i = 0; i < vehiclePositionListSize; i++)
            {
                if (_vehiclePositionList[i].CurrentLap == lap)
                {
                    tempList.Add(_vehiclePositionList[i]);
                    tempIndex.Add(i);
                }
            }

            // Sort based on checkpoint
            tempList = tempList.OrderBy(x => -x.CurrentCheckpoint).ToList();

            // Add back to original list
            for (int i = 0; i < tempList.Count; i++)
            {
                _vehiclePositionList[tempIndex[i]] = tempList[i];
            }
        }

        foreach (VehiclePosition vehiclePosition in _vehiclePositionList)
        {
            if (vehiclePosition.CurrentLap == _numberOfLaps) continue;
            Team team = vehiclePosition.Vehicle.GetComponent<Player>().Team;

            //TargetPositionUpdate(vehiclePosition.Vehicle.GetComponent<NetworkIdentity>().connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
            if (team.DriverIdentity.connectionToClient != null) TargetPositionUpdate(team.DriverIdentity.connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
            if (team.GunnerIdentity.connectionToClient != null) TargetPositionUpdate(team.GunnerIdentity.connectionToClient, _vehiclePositionList.IndexOf(vehiclePosition) + 1);
        }
    }

    [TargetRpc]
    private void TargetPositionUpdate(NetworkConnection target, int position)
    {
        OnPositionUpdate?.Invoke(position);
    }

    [TargetRpc]
    private void TargetFinishRace(NetworkConnection target, int position)
    {
        FinishRace?.Invoke(position, "You finished the match. Your team's position: ");
    }
}