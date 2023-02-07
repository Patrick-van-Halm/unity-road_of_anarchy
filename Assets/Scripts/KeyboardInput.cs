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
    private bool _settingsOpen;

    private bool _canUse;
    private MenuManager _menuManager;

    public bool Accelerating { get { return _canUse && _accelerating; } }
    public bool Braking { get { return _canUse && _braking; } }
    public bool Shooting { get { return _canUse && _shooting; } }
    public bool Reload { get { return _canUse && _reload; } }
    public float SteerInput { get { return _canUse ? _steerInput : 0f; } }
    public bool SettingsOpen { get { return _settingsOpen; } }

    private void Start()
    {
        _menuManager = FindObjectOfType<MenuManager>();
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

    public void OnToggleSettings(InputValue value)
    {
        // Opens/closes panel based on settings panel visible
        _settingsOpen = !_menuManager.SettingsPanel.activeInHierarchy;
        _menuManager.TogglePanel(_menuManager.SettingsPanel);

        // Save settings if player decides to close window with ESC (instead of close-button)
        _menuManager.GetComponent<Settings>().Save();

        // Cursor behaviour based on settings panel visible
        if (_settingsOpen) { Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }
        else { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }
    }
}
