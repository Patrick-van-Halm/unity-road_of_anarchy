using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerCameraController : MonoBehaviour
{
    public Camera gunnerCamera;

    [Header("Camera settings")]
    public float mouseSensitivity;
    public float minXRotation;
    public float maxXRotation;
    
    private float xRotation;
    private float yRotation;

    void Start()
    {
        xRotation = 0f;
        yRotation = 0f;

        // Lock and hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse delta. This is not in the range -1...1
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Set rotation, if necessary invert mouse rotation
        xRotation -= mouseY;
        yRotation += mouseX;

        // Clamp up and down viewing range
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

        // Set camera rotation
        gunnerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
