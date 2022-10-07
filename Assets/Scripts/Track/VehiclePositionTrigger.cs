using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class VehiclePositionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _frontPositionTriggerObject;
    [SerializeField] private GameObject _backPositionTriggerObject;
    [SerializeField] private float _constraintOffset;

    private RaceManager _raceManager;
    private Transform _vehicleTransform;
    private ParentConstraint _parentConstraint;
    private ConstraintSource _constraintSource;


    private void Start()
    {
        _parentConstraint = GetComponent<ParentConstraint>();

        // Set car transform as constraint (Transform to follow)
        _constraintSource.sourceTransform = _vehicleTransform.transform;
        _constraintSource.weight = 1;
        _parentConstraint.AddSource(_constraintSource);

        // Constraint offset
        _parentConstraint.SetTranslationOffset(0, _vehicleTransform.forward * _constraintOffset);

        // Turn on parentConstraint
        _parentConstraint.weight = 1f;
        _parentConstraint.constraintActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If not colliding with itself
        Transform[] temp = other.GetComponentsInParent<Transform>();
        if (!temp.Contains(_vehicleTransform))
        {
            // Get vehicle transform with which we are colliding
            Car[] car = other.GetComponentsInParent<Car>();
            Transform collisionVehicleTransform = car[0].transform;

            //When triggers on vehicle gets hit
            if (other.name == _frontPositionTriggerObject.name)
            {
                _raceManager.VehicleThroughPositionCollider(_vehicleTransform, collisionVehicleTransform, true, true);
            }
            else if (other.name == _backPositionTriggerObject.name)
            {
                _raceManager.VehicleThroughPositionCollider(_vehicleTransform, collisionVehicleTransform, false, true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If not colliding with itself
        Transform[] temp = other.GetComponentsInParent<Transform>();
        if (!temp.Contains(_vehicleTransform))
        {
            // Get vehicle transform with which we are colliding
            Car[] car = other.GetComponentsInParent<Car>();
            Transform collisionVehicleTransform = car[0].transform;

            //When triggers on vehicle gets hit
            if (other.name == _frontPositionTriggerObject.name)
            {
                _raceManager.VehicleThroughPositionCollider(_vehicleTransform, collisionVehicleTransform, true, false);
            }
            else if (other.name == _backPositionTriggerObject.name)
            {
                _raceManager.VehicleThroughPositionCollider(_vehicleTransform, collisionVehicleTransform, false, false);
            }
        }
    }

    /// <summary>
    /// Sets values in this script
    /// </summary>
    /// <param name="raceManagerScript"></param>
    public void SetValues(RaceManager raceManagerScript, Transform vehicleTransform)
    {
        _raceManager = raceManagerScript;
        this._vehicleTransform = vehicleTransform;
    }

    /// <summary>
    /// Set the size and rotation of the collider attached to the vehicle. 
    /// This collider is used for the position between vehicles when they collide with each other.
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="scale"></param>
    public void SetTransformSize(Quaternion rotation, Vector3 scale)
    {
        transform.rotation = rotation;
        transform.localScale = new Vector3(scale.x, scale.y, transform.localScale.z);
    }
}
