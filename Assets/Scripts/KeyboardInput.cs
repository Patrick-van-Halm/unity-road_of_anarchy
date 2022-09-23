using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    private bool _accelerating;
    private bool _braking;
    private float _steerInput;

    public bool Accelerating { get { return _accelerating; } }
    public bool Braking { get { return _braking; } }
    public float SteerInput { get { return _steerInput; } }

    public void OnAccelerating(InputValue value)
    {
        _accelerating = value.isPressed;
    }

    public void OnBraking(InputValue value)
    {
        _braking = value.isPressed;
    }

    public void OnSteering(InputValue value)
    {
        _steerInput = value.Get<float>();
    }
}
