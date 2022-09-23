using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarAirTiltMechanic : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private CarInput _playerInput;

    [Header("Tilt settings")]
    [SerializeField] private LayerMask _environmentLayer;
    [SerializeField] private float _tiltMultiplier;

    [Header("SFX")]
    [SerializeField] private CarCrashSfx _carCrashSfx;
    [SerializeField] private CarFallingSfx _carFallingSfx;

    private Vector2 _tiltDirection;

    private bool _onGround = false;
    private bool _lastOnGround = true;

    private float _highestRawVolume = 0;
    private readonly float _maxRawVolume = 30f;
    private float _actualVolume = 0;
    private readonly float _maxActualVolume = 1f;

    private void Update()
    {
        CarFallingSfxHandler();

        // checks if the car is in the air
        if (!_onGround)
        {
            CarSfxVolumeHandler();
            ProcessInput();
            TiltMomentumHandler();
        }
    }

    /// <summary>
    /// Player will hear either wind or impact noises based on the car just flying through the air or just landing on the ground.
    /// </summary>
    private void CarFallingSfxHandler()
    {
        if (_onGround != _lastOnGround)
        {
            if (!_onGround)
            {
                _highestRawVolume = 0;
                _carFallingSfx.ActivateInstance();
            }
            else
            {
                _carFallingSfx.DeactivateInstance();
                _carCrashSfx.ActivateInstance();
            }

            _lastOnGround = _onGround;
        }
    }

    /// <summary>
    /// Sets volume of the sound effects based on the velocity of the car.
    /// </summary>
    private void CarSfxVolumeHandler()
    {
        // just the combined velocity of the car.
        float _rawVolume = _playerRB.velocity.magnitude;

        // only the highest velocity will be stored.
        if (_highestRawVolume < _rawVolume)
            _highestRawVolume = _rawVolume;

        // turn velocity into actual volume.
        _actualVolume = _maxActualVolume / _maxRawVolume * _highestRawVolume;
        _actualVolume = Mathf.Clamp(_actualVolume, 0, _maxActualVolume);

        _carCrashSfx.SetSfxVolume(_actualVolume);
        _carFallingSfx.SetSfxVolume(_actualVolume);
    }

    /// <summary>
    /// Calculates the direction corresponding to the input of the player and stores it in <b>_tiltDirection</b>
    /// </summary>
    private void ProcessInput()
    {
        float x = 0;
        float y = 0;

        if (_playerInput.Accelerating) x += 1;
        if (_playerInput.Braking) x -= 1;
        //if (_input.SteerLeft) y += 1;
        if (Input.GetKey(KeyCode.A)) y += 1;
        //if (_input.SteerRight) y -= 1;
        if (Input.GetKey(KeyCode.D)) y -= 1;

        _tiltDirection = new Vector2(x, y).normalized;
    }

    /// <summary>
    /// Adds relative torque to rigidbody of player.
    /// </summary>
    private void TiltMomentumHandler()
    {
        _playerRB.AddRelativeTorque(new Vector3(_tiltDirection.x, 0, _tiltDirection.y) * _tiltMultiplier);
    }

    /// <summary>
    /// Tracks if the player is colliding with the ground (environment) and when the player does, the boolean <b>onGround</b> will be true else false.
    /// </summary>
    /// <param name="collision">Any collision that the player is colliding with.</param>
    private void OnCollisionStay(Collision collision)
    {
        if (_environmentLayer == (1 << collision.gameObject.layer))
            _onGround = true;
        else
            _onGround = false;
    }
}
