using UnityEngine;

public class CarTiltMechanic : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private KeyboardInput _playerInput;
    [SerializeField] private NewKartScript _kartScript;
    [SerializeField] private CarInfo _carInfo;

    [Header("Tilt settings")]
    [SerializeField] private float _airTiltMultiplier;
    [SerializeField] private float _groundTiltMultiplier;
    [SerializeField] private float _tiltMultiplier = 1000;

    [Header("SFX")]
    [SerializeField] private CarCrashSfx _carCrashSfx;
    [SerializeField] private CarFallingSfx _carFallingSfx;
    [SerializeField] private CarRollingSfx _carRollingSfx;
    [SerializeField] private CarWheelsGroundedSfx _carWheelsGroundedSfx;

    private float _highestRawVolume = 0;
    private readonly float _maxRawVolume = 100f;
    private float _actualVolume = 0;
    private readonly float _maxActualVolume = 1f;

    private Vector2 _tiltDirection;

    /// <summary>
    /// Updates every method
    /// </summary>
    private void Update()
    {
        ProcessInput();
        CarStartToFlyHandler();
        GroundTiltHandler();
        CarStartToTouchGroundHandler();
        if (!_carInfo.WholeCarGrounded)
        {
            CarSfxVolumeHandler();
            AirTiltHandler();
        }
    }

    /// <summary>
    /// Calculates the direction corresponding to the input of the player and stores it in <b>_tiltDirection</b>
    /// </summary>
    private void ProcessInput()
    {
        float x = 0;
        if (_playerInput.Accelerating) x += 1;
        if (_playerInput.Braking) x -= 1;

        float y = -_playerInput.SteerInput;

        _tiltDirection = new Vector2(x, y).normalized;
    }

    /// <summary>
    /// When the car starts to fly. <br/>
    /// - Sets move (from NewKartScript) to false <br/>
    /// - Resets the sfx volume and starts the falling sfx.
    /// </summary>
    private void CarStartToFlyHandler()
    {
        if (_carInfo.LastWholeCarGrounded && !_carInfo.WholeCarGrounded)
        {
            _highestRawVolume = 0;
            _carFallingSfx.ActivateInstance();
            _kartScript.SetCanMove(false);
        }
    }

    /// <summary>
    /// When the car starts to touch the ground. <br/>
    /// - Sets move (from NewKartScript) to true <br/>
    /// - Stops the falling sfx and starts the crash sfx.
    /// </summary>
    private void CarStartToTouchGroundHandler()
    {
        if (!_carInfo.LastWholeCarGrounded && _carInfo.WholeCarGrounded)
        {
            _carFallingSfx.DeactivateInstance();
            _carCrashSfx.ActivateInstance();
            _kartScript.SetCanMove(true);
        }
    }

    /// <summary>
    /// Sets volume of the sound effects based on the velocity of the player's car.
    /// </summary>
    private void CarSfxVolumeHandler()
    {
        // just the combined velocity of the car.
        float _rawVolume = _playerRB.velocity.magnitude;

        // only the highest velocity will be stored.
        if (_highestRawVolume < _rawVolume) _highestRawVolume = _rawVolume;

        // turn velocity into actual volume.
        _actualVolume = _maxActualVolume / _maxRawVolume * _highestRawVolume;
        _actualVolume = Mathf.Clamp(_actualVolume, 0, _maxActualVolume);

        _carCrashSfx.SetSfxVolume(_actualVolume);
        _carFallingSfx.SetSfxVolume(_actualVolume);
    }

    /// <summary>
    /// Adds relative torque to rigidbody of player's car.
    /// </summary>
    private void AirTiltHandler()
    {
        _playerRB.AddRelativeTorque(new Vector3(_tiltDirection.x, 0, _tiltDirection.y) * _airTiltMultiplier * _tiltMultiplier * Time.deltaTime);
    }

    /// <summary>
    /// Adding torque to the rigidbody to tilt the car until some wheels touch the ground <br/>
    /// When some wheels touch the ground the <b>AirborneReorientationCoefficient</b> from NewKartScript will be used to full rotate the car on all wheels. <br/>
    /// Sound of the car rotating will also be heard accordingly.
    /// </summary>
    private void GroundTiltHandler()
    {
        _kartScript.AirborneReorientationCoefficient = 0;
        if (_carInfo.WholeCarGrounded && !_carInfo.AllWheelsGrounded && _carInfo.AbleToGroundTiltInDirection(_tiltDirection.y))
        {
            if (!_carInfo.SomeWheelsGrounded) _playerRB.AddRelativeTorque(new Vector3(0, 0, _tiltDirection.y) * _groundTiltMultiplier * _tiltMultiplier * Time.deltaTime);
            _carRollingSfx.ActivateInstance();
        }
        else
        {
            if (_carInfo.AllWheelsGrounded != _carInfo.LastAllWheelsGrounded) _carWheelsGroundedSfx.ActivateInstance();
            _carRollingSfx.DeactivateInstance();
        } 
    }
}
