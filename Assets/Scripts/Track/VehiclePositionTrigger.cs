using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class VehiclePositionTrigger : MonoBehaviour
{
    [SerializeField] private string _frontPositionTriggerTag;
    [SerializeField] private string _backPositionTriggerTag;
    [SerializeField] private float _constraintOffset;

    private Transform _checkpointTransform;
    private Transform _vehicleTransform;
    private ParentConstraint _parentConstraint;
    private ConstraintSource _constraintSource;

    private void Start()
    {
        _parentConstraint = GetComponent<ParentConstraint>();
        _vehicleTransform = transform.parent;
        if (_checkpointTransform == null) _checkpointTransform = _vehicleTransform;

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
        OnTriggerChange(other, other.CompareTag(_frontPositionTriggerTag), true);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerChange(other, other.CompareTag(_frontPositionTriggerTag), false);
    }

    private void Update()
    {
        SetRotationOffset();
    }

    private void SetRotationOffset()
    {
        // Calculate rotation offset so the car can rotate freely without changing the rotation of the this (VehiclePositionTrigger) collider.
        float newRotationOffsetY = _checkpointTransform.localEulerAngles.y - _vehicleTransform.localEulerAngles.y;
        Vector3 newRotationOffset = new Vector3(0, newRotationOffsetY, 0);
        _parentConstraint.SetRotationOffset(0, newRotationOffset);
    }

    private void OnTriggerChange(Collider other, bool isFrontTrigger, bool hasEnteredTrigger)
    {
        if (!other.CompareTag(_frontPositionTriggerTag) && !other.CompareTag(_backPositionTriggerTag)) return;

        // Get vehicle transform with which we are colliding
        Transform collisionVehicleTransform = other.transform.parent;

        // If not colliding with itself
        if (collisionVehicleTransform != null && collisionVehicleTransform.CompareTag("Vehicle"))
        {
            RaceManager.Instance.CmdVehicleThroughPositionCollider(collisionVehicleTransform.gameObject, _vehicleTransform.gameObject, isFrontTrigger, hasEnteredTrigger);
        }
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

    public void SetCheckpointTransform(Transform checkpointTransform)
    {
        _checkpointTransform = checkpointTransform;
    }
}
