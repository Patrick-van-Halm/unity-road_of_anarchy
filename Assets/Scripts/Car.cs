using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float _currentSpeed = 0f;
    private float _acceleration = 300f;
    private float _deceleration = 30f;

    public float Acceleration { get { return _acceleration; } }
    private float _speedMod = 1000f;

    private bool _isBraking;

    [SerializeField] private ParameterSetter ParameterSetter;
    [SerializeField] private CarInput _input;
    [SerializeField] private Rigidbody _car;

    [Header("Wheels")]
    [SerializeField] private List<WheelCollider> _wheelColliders;

    private void Update()
    {
        Accelerate();

        // Calculating speed of car
        var forwardSpeed = _car.velocity.z;
        _currentSpeed = forwardSpeed * 3.6f;

        // Apply soundFX
        ParameterSetter._speed = _currentSpeed * 2.5f;
    }

    /// <summary>
    /// Accelerates car. Checks if car isn't already braking and applies correct acceleration to car. 
    /// Depending on user input, the car will either accelerate or slowly lose speed (and eventually stand still).
    /// </summary>
    public void Accelerate()
    {
        if (!_isBraking)
        {
            if (_input.Accelerating)
            {
                // Applying acceleration to all wheels
                foreach (WheelCollider wheel in _wheelColliders)
                    wheel.motorTorque = (_acceleration - _currentSpeed * 8f) * _speedMod * Time.deltaTime;
                foreach (WheelCollider wheel in _wheelColliders)
                    wheel.brakeTorque = 0f;
            }
            else
            {
                // Applying deceleration to all wheels
                foreach (WheelCollider wheel in _wheelColliders)
                    wheel.brakeTorque = _deceleration * _speedMod * Time.deltaTime;
                foreach (WheelCollider wheel in _wheelColliders)
                    wheel.motorTorque = 0f;
            }
        }
    }
}
