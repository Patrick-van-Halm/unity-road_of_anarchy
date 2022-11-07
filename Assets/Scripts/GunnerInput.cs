using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunnerInput : MonoBehaviour
{
    private Vector2 _lookInput;

    public Vector2 LookInput { get { return _lookInput; } }
    public void OnMouse(InputValue value)
    {
        _lookInput = (value.Get<Vector2>());
    }
}
