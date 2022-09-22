using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float _currentSpeed = 0f;
    private float _acceleration = 300f;
    private float _deceleration = 30f;
    private float _braking = 300f;
    private float _reverse = 50f;

    public float CurrentSpeed { get { return _currentSpeed; } set { _currentSpeed = value; } }

    private float _speedMod = 1000f;

    [SerializeField] private ParameterSetter ParameterSetter;
    [SerializeField] private SoundFX SoundFX;
    [SerializeField] private CarInput _input;
    [SerializeField] private Rigidbody _car;

    [Header("Wheels")]
    [SerializeField] private List<WheelCollider> _wheelColliders;

    private void Update()
    {
        Accelerate();
        Decelerate();
        Brake();

        // Calculating speed of car
        var forwardSpeed = _car.velocity.z;
        _currentSpeed = forwardSpeed * 3.6f;

        // Apply speed soundFX
        ParameterSetter._speed = _currentSpeed * 2.5f;

        // Stop brake sound
        if (_currentSpeed < 5f) SoundFX.StopBrakeSfx();
    }

    /// <summary>
    /// Accelerates car. Checks if car isn't already braking and applies acceleration to car. 
    /// </summary>
    public void Accelerate()
    {
        if (_input.Accelerating && !_input.Braking)
        {
            // Stop brake soundFX
            SoundFX.StopBrakeSfx();

            // Applying acceleration to all wheels
            foreach (WheelCollider wheel in _wheelColliders)
            {
                wheel.motorTorque = (_acceleration - _currentSpeed * 4f) * _speedMod * Time.deltaTime;
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
            // Stop brake soundFX
            SoundFX.StopBrakeSfx();

            // Applying deceleration to all wheels
            foreach (WheelCollider wheel in _wheelColliders)
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
            // Play brake sound
            if (_currentSpeed > 10f) SoundFX.PlayBrakeSfx();

            // Applying braking to all wheels
            foreach (WheelCollider wheel in _wheelColliders)
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
}
