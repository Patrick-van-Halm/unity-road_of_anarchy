using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    private bool _accelerating;
    private bool _braking;
    private bool _reload;
    private bool _shooting;
    private float _steerInput;

    private bool _canUse;

    public bool Accelerating { get { return _canUse && _accelerating; } }
    public bool Braking { get { return _canUse && _braking; } }
    public bool Shooting { get { return _canUse && _shooting; } }
    public bool Reload { get { return _canUse && _reload; } }
    public float SteerInput { get { return _canUse ? _steerInput : 0f; } }

    private void Start()
    {
        CountdownManager.Instance?.CountdownEnds.AddListener(InputAllowed);
    }

    public void InputAllowed()
    {
        _canUse = true;
    }

    public void OnAccelerating(InputValue value)
    {
          _accelerating = value.isPressed;
    }

    public void OnBraking(InputValue value)
    {
         _braking = value.isPressed;
    }

    public void OnShooting(InputValue value)
    {
         _shooting = value.isPressed;
    }

    public void OnReload(InputValue value)
    {
         _reload = value.isPressed;
    }

    public void OnSteering(InputValue value)
    {
        _steerInput = value.Get<float>();
    }
}
