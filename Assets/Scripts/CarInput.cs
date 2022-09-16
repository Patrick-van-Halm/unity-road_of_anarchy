using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private bool _accelerating;
    public bool Accelerating { get { return _accelerating; } set { _accelerating = value; } }

    public void OnAccelerating(InputValue value)
    {
        _accelerating = value.isPressed;
    }
}
