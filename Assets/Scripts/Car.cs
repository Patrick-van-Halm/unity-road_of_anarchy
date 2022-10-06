using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float _currentSpeed = 0f;
    private float _speedMod = 1000f;
    private float _maxSpeed = 50f;
    private float _acceleration = 300f;
    private float _deceleration = 30f;
    private float _braking = 300f;
    private float _reverse = 50f;

    private float _highSpeedSteerHandle = 100f;
    public float HighSpeedSteerHandle { get { return _highSpeedSteerHandle; } }
    private float _lowSpeedSteerHandle = 35f;

    public float CurrentSpeed { get { return _currentSpeed; } set { _currentSpeed = value; } }

    [SerializeField] private VehicleAudio ParameterSetter;
    [SerializeField] private CarInput _input;
    [SerializeField] private Rigidbody _car;

    [Header("Wheels")]
    [SerializeField] private List<WheelCollider> _allWheels;
    [SerializeField] private List<WheelCollider> _steerableWheels;
    [SerializeField] public List<WheelCollider> SteerableWheels { get { return _steerableWheels; } }

    private void Start()
    {
        _car.centerOfMass = new Vector3(0, -1f, 0);
    }

    private void Update()
    {
        Accelerate();
        Decelerate();
        Brake();
        Steer();

        // Calculating speed of car
        var forwardSpeed = _car.velocity.magnitude;
        _currentSpeed = forwardSpeed * 3.6f;

        // Apply speed soundFX
        ParameterSetter.SetSpeed(_currentSpeed * 2.5f);
    }


    /// <summary>
    /// Accelerates car. Checks if car isn't already braking and applies acceleration to car. 
    /// </summary>
    public void Accelerate()
    {
        if (_input.Accelerating && !_input.Braking)
        {

            // Applying acceleration to all wheels
            foreach (WheelCollider wheel in _allWheels)
            {
                wheel.motorTorque = (_acceleration - _currentSpeed * 8f) * _speedMod * Time.deltaTime;
                wheel.brakeTorque = 0f;
            }
        }
    }
    /// <summary>
    /// Decerlates car. Checks if car is accelerating and braking or not accelerating and braking and applies deceleration to car.
    /// </summary>
    public void Decelerate()
    {
        if (!_input.Accelerating && !_input.Braking || _input.Accelerating && _input.Braking)
        {

            // Applying deceleration to all wheels
            foreach (WheelCollider wheel in _allWheels)
            {
                wheel.brakeTorque = _deceleration * _speedMod * Time.deltaTime;
                wheel.motorTorque = 0f;
            }
        }
    }

    /// <summary>
    /// Brakes car. Checks if car is braking and applies braking to car. Car will reverse if car is standing still.
    /// </summary>
    public void Brake()
    {
        if (_input.Braking && !_input.Accelerating)
        {

            // Applying braking to all wheels
            foreach (WheelCollider wheel in _allWheels)
            {
                if (_currentSpeed <= 2f)
                {
                    wheel.brakeTorque = 0f;
                    wheel.motorTorque = (-_reverse - _currentSpeed * 1.5f) * _speedMod * Time.deltaTime;
                }
                else
                {
                    wheel.brakeTorque = _braking * _speedMod * Time.deltaTime;
                    wheel.motorTorque = 0f;
                }
            }
        }
    }

    public void Steer()
    {
        foreach (WheelCollider wheel in _steerableWheels)
        {
            float steeringAngle = _input.SteerInput * _highSpeedSteerHandle;
            wheel.steerAngle = steeringAngle;
        }
    }

}
