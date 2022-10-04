using System.Collections;
using UnityEngine;

public class GunnerCameraController : MonoBehaviour
{
    public Camera gunnerCamera;

    [Header("Camera settings")]
    public float mouseSensitivity;
    public float minXRotation;
    public float maxXRotation;

    [SerializeField] ParameterSetter _parameterSetter = new ParameterSetter();
    
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

        StartCoroutine(CheckRotationValues());
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse delta. This is not in the range -1...1
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Set rotation, if necessary invert mouse rotation
        _xRotation -= mouseY;
        _yRotation += mouseX;

        // Clamp up and down viewing range
        _xRotation = Mathf.Clamp(_xRotation, minXRotation, maxXRotation);

        // Set camera rotation
        gunnerCamera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }

    IEnumerator CheckRotationValues()
    {
        while (true)
        {
            // Check rotation values to play audio
            if (_lastRotate != gunnerCamera.transform.localRotation)
            {
                _parameterSetter.PlayIsMovingSFX();
                _lastRotate = gunnerCamera.transform.localRotation;
            }
            else _parameterSetter.StopIsMovingSFX();

            yield return new WaitForSeconds(0.2f);
        }
    }
}
