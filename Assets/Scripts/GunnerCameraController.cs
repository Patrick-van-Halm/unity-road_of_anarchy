using System.Collections;
using UnityEngine;

public class GunnerCameraController : MonoBehaviour
{
    public Camera gunnerCamera;
    public GunnerAudio audio;
    [SerializeField] private GameSettings _gameSettings;

    [Header("Camera settings")]
    [SerializeField] private float _minXRotation;
    [SerializeField] private float _maxXRotation;
    [SerializeField] private float _sensitivityMod;

    [SerializeField] private GunnerInput _gunnerInput;

    private float _xRotation;
    private float _yRotation;

    private Quaternion _lastRotate;

    void Start()
    {
        _xRotation = 0f;
        _yRotation = 0f;

        // Lock and hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Set rotation values
        _lastRotate = gunnerCamera.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float oldMouseX = 0;
        float oldMouseY = 0;

        // Get the mouse delta. This is not in the range -1...1
        float mouseX = _gunnerInput.LookInput.x * (_gameSettings.Sensitivity * _sensitivityMod) * Time.deltaTime;
        float mouseY = _gunnerInput.LookInput.y * (_gameSettings.Sensitivity * _sensitivityMod) * Time.deltaTime;

        // Check mouse movement for rotation soundFX
        if (mouseX != oldMouseX || mouseY != oldMouseY) audio.PlayIsMovingSFX();
        else audio.StopIsMovingSFX();

        // Set rotation, if necessary invert mouse rotation
        if (_gameSettings.InvertX) _yRotation -= mouseX; // inverted x
        else _yRotation += mouseX;
        if (_gameSettings.InvertY) _xRotation += mouseY;
        else _xRotation -= mouseY; // inverted y

        // Clamp up and down viewing range
        _xRotation = Mathf.Clamp(_xRotation, _minXRotation, _maxXRotation);

        // Set camera rotation
        gunnerCamera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}
