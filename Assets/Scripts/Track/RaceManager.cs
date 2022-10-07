using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RaceManager : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private List<Transform> _vehicleTransformList;
    [SerializeField] private int _numberOfLaps;

    [Header("Object triggers on vehicle")]
    [SerializeField] private GameObject _frontVehiclePositionTriggerObject;

    private List<VehiclePositionTrigger> _vehiclePositionTriggerList;
    private List<Checkpoint> _checkpointsList;
    private List<VehiclePosition> _vehiclePositionList;
    private List<DynamicOvertakeTrigger> _dynamicOvertakeTriggerList;

    private class VehiclePosition
    {
        public Transform VehicleTransform { get; set; }
        public int CurrentCheckpoint { get; set; }
        public int CurrentLap { get; set; }
    }

    private class DynamicOvertakeTrigger
    {
        public Transform CurrentVehicleTransform { get; set; }
        public Transform CollisionVehicleTransform { get; set; }
        public bool IsFrontTrigger { get; set; }
    }

    public UnityEvent WrongCheckpoint;
    public UnityEvent CorrectCheckpoint;
    public UnityEvent<int> SetNumberOfLaps;
    public UnityEvent<int> IncreaseLap;
    public UnityEvent<int> OnPositionUpdate;
    public UnityEvent<int> OnSwapPosition;

    private void Awake()
    {
        // Initialization
        _vehiclePositionTriggerList = new List<VehiclePositionTrigger>();
        _checkpointsList = new List<Checkpoint>();
        _vehiclePositionList = new List<VehiclePosition>();
        _dynamicOvertakeTriggerList = new List<DynamicOvertakeTrigger>();

        // Set starting values 
        for (int i = 0; i < _vehicleTransformList.Count; i++)
        {
            _vehiclePositionList.Add(new VehiclePosition { VehicleTransform = _vehicleTransformList[i], CurrentLap = 0, CurrentCheckpoint = 0 });
        }

        // Get child transforms attached to TrackCheckpoints gameobject
        Transform allCheckpointsTransform = transform.Find("TrackCheckpoints");

        // Cycles through all of the children from the above selected (Get all checkpoints under TrackCheckPoints)
        foreach(Transform checkpointTransform in allCheckpointsTransform)
        {
            // Get checkpoint script attached to the checkpoint gameobject
            Checkpoint checkpoint = checkpointTransform.GetComponent<Checkpoint>();

            // Pass this script to Checkpoint script
            checkpoint.SetRaceManagerScript(this);

            // Add found checkpoints to list
            _checkpointsList.Add(checkpoint);
        }

        // Cycles through all the vehicles and passes a reference of this script
        foreach (Transform vehicleTransform in _vehicleTransformList)
        {
            // Instantiate object collider on vehicle
            GameObject frontVehiclePositionTrigger = Instantiate(_frontVehiclePositionTriggerObject, vehicleTransform.position, vehicleTransform.rotation);

            // Get VehiclePositionCollider Script
            VehiclePositionTrigger frontVehiclePositionTriggerScript = frontVehiclePositionTrigger.GetComponent<VehiclePositionTrigger>();

            // Set values in vehiclePositionTrigger object
            frontVehiclePositionTriggerScript.SetValues(this, vehicleTransform);

            // Store all trigger objects on the vehicle
            _vehiclePositionTriggerList.Add(frontVehiclePositionTriggerScript);
        }
    }

    private void Start()
    {
        SetNumberOfLaps?.Invoke(_numberOfLaps);
    }

    /// <summary>
    /// This function gets called from a checkpoint gameobject when a vehicle collides with it. 
    /// This method is responsible for checking if the vehicle went through the right checkpoint. 
    /// Futhermore, it will trigger another method (CheckRacePosition) which will update the vehicle positions.
    /// </summary>
    /// <param name="checkpoint"></param>
    /// <param name="vehicleTransform"></param>
    public void VehicleThroughCheckPoint(Checkpoint checkpoint, Transform vehicleTransform)
    {
        // Get right index of current vehicle in sorted list
        int vehicleIndex = 0;
        for (int i = 0; i < _vehiclePositionList.Count; i++)
        {
            if(vehicleTransform == _vehiclePositionList[i].VehicleTransform)
            {
                vehicleIndex = i;
                break;
            }
        }

        // Check if vehicle is on the right checkpoint
        if (_checkpointsList.IndexOf(checkpoint) == _vehiclePositionList[vehicleIndex].CurrentCheckpoint)
        {
            // Increase checkpoint counter for that vehicle
            _vehiclePositionList[vehicleIndex].CurrentCheckpoint++;

            // Correct checkpoint event
            CorrectCheckpoint?.Invoke();

            // When lap is completed
            if (_checkpointsList.Count == _vehiclePositionList[vehicleIndex].CurrentCheckpoint)
            {
                _vehiclePositionList[vehicleIndex].CurrentLap++;               // Increase lap for vehicle                
                _vehiclePositionList[vehicleIndex].CurrentCheckpoint = 0;      // Reset checkpoint

                // Increaselap event
                IncreaseLap?.Invoke(_vehiclePositionList[vehicleIndex].CurrentLap);
            }
        }
        else
        {
            // Wrong checkpoint
            WrongCheckpoint?.Invoke();

            // Decrease checkpoint counter for that vehicle
            int lastCheckpoint = _vehiclePositionList[vehicleIndex].CurrentCheckpoint - 1;
            if (lastCheckpoint == _checkpointsList.IndexOf(checkpoint)) _vehiclePositionList[vehicleIndex].CurrentCheckpoint--;
            if (_checkpointsList.IndexOf(checkpoint) == 0) _vehiclePositionList[vehicleIndex].CurrentCheckpoint++;
        }

        // Set the position of the vehicle
        CheckRacePosition();

        // Update rotation of the trigger collider on the vehicle
        _vehiclePositionTriggerList[vehicleIndex].SetTransformSize(checkpoint.transform.rotation, checkpoint.transform.localScale);
    }

    /// <summary>
    /// This function is responsible for overtaking between the vehicles itself. It will be called from a trigger which is attached to the vehicles 
    /// </summary>
    /// <param name="currentVehicleTransform"></param>
    /// <param name="collisionVehicleTransform"></param>
    /// <param name="isFrontTrigger"></param>
    /// <param name="hasEnteredTrigger"></param>
    public void VehicleThroughPositionCollider(Transform currentVehicleTransform, Transform collisionVehicleTransform, bool isFrontTrigger, bool hasEnteredTrigger)
    {
        if(hasEnteredTrigger)
        {
            _dynamicOvertakeTriggerList.Add(new DynamicOvertakeTrigger() { CurrentVehicleTransform = currentVehicleTransform, CollisionVehicleTransform = collisionVehicleTransform, IsFrontTrigger = isFrontTrigger});
        }
        else
        {
            // Get all items from the list where currentvehicle collides with collisionvehicle
            List<DynamicOvertakeTrigger> tempList = new List<DynamicOvertakeTrigger>();
            for (int i = 0; i < _dynamicOvertakeTriggerList.Count; i++)
            {
                // Only store the currentvehicle and collisionvehicles
                if(_dynamicOvertakeTriggerList[i].CurrentVehicleTransform == currentVehicleTransform && _dynamicOvertakeTriggerList[i].CollisionVehicleTransform == collisionVehicleTransform ||
                   _dynamicOvertakeTriggerList[i].CurrentVehicleTransform == collisionVehicleTransform && _dynamicOvertakeTriggerList[i].CollisionVehicleTransform == currentVehicleTransform)
                {
                    tempList.Add(_dynamicOvertakeTriggerList[i]);
                }
            }

            // Remove duplicates
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                if (i >= tempList.Count) continue;
                DynamicOvertakeTrigger overtakeTrigger = tempList[i];
                tempList.RemoveAll(o => o.CollisionVehicleTransform == overtakeTrigger.CollisionVehicleTransform && o.CurrentVehicleTransform == overtakeTrigger.CurrentVehicleTransform && o.IsFrontTrigger == overtakeTrigger.IsFrontTrigger);
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
            if (isFrontCounter + isBackCounter == 4)
            {
                SwapVehiclePositions(currentVehicleTransform, collisionVehicleTransform);

                // Find all vehicles which need to be removed
                List<int> removeVehicleIndexList = new List<int>();
                for (int i = 0; i < _dynamicOvertakeTriggerList.Count; i++)
                {
                    if (_dynamicOvertakeTriggerList[i].CurrentVehicleTransform == currentVehicleTransform && _dynamicOvertakeTriggerList[i].CollisionVehicleTransform == collisionVehicleTransform ||
                        _dynamicOvertakeTriggerList[i].CurrentVehicleTransform == collisionVehicleTransform && _dynamicOvertakeTriggerList[i].CollisionVehicleTransform == currentVehicleTransform)
                    {
                        removeVehicleIndexList.Add(i);
                    }
                }

                // Remove all vehicles
                for (int indexToRemove = removeVehicleIndexList.Count - 1; indexToRemove >= 0; indexToRemove--)
                {
                    _dynamicOvertakeTriggerList.RemoveAt(removeVehicleIndexList[indexToRemove]);
                }

                OnSwapPosition?.Invoke(1);
            }
        }
    }

    private void SwapVehiclePositions(Transform currentVehicleTransform, Transform collisionVehicleTransform)
    {
        //Duplicates exist car is overtaken
        int firstVehicleIndex = 0;
        int secondVehicleIndex = 0;

        // Get collided vehicle indexes
        for (int i = 0; i < _vehiclePositionList.Count; i++)
        {
            if (_vehiclePositionList[i].VehicleTransform == currentVehicleTransform)
            {
                firstVehicleIndex = i;
            }

            if (_vehiclePositionList[i].VehicleTransform == collisionVehicleTransform)
            {
                secondVehicleIndex = i;
            }
        }

        //Swap vehicle positions around in list
        List<VehiclePosition> tempList = new List<VehiclePosition> { _vehiclePositionList[firstVehicleIndex] };
        _vehiclePositionList[firstVehicleIndex] = _vehiclePositionList[secondVehicleIndex];
        _vehiclePositionList[secondVehicleIndex] = tempList[0];
    }

    private void CheckRacePosition()
    {
        // Sort based on lap
        _vehiclePositionList = _vehiclePositionList.OrderBy(x => -x.CurrentLap).ToList();

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

        OnPositionUpdate?.Invoke(1);
    }
}