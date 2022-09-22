using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private bool _accelerating;
    private bool _braking;
    public bool Accelerating { get { return _accelerating; } set { _accelerating = value; } }
    public bool Braking { get { return _braking; } set { _braking = value; } }

    public void OnAccelerating(InputValue value)
    {
        _accelerating = value.isPressed;
    }

    public void OnBraking(InputValue value)
    {
        _braking = value.isPressed;
    }
}
