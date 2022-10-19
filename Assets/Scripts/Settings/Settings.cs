using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    public void SetUsername(string value)
    {
        _gameSettings.Username = value;
    }

    public void SetAudioVolume(float value)
    {
        _gameSettings.AudioVolume = value;

    }

    public void SetInvertX(bool value)
    {
        _gameSettings.InvertX = value;
    }

    public void SetInvertY(bool value)
    {
        _gameSettings.InvertY = value;
    }

    public void SetSensitivity(float value)
    {
        _gameSettings.Sensitivity = value;
    }

    public void SetAutomaticReload(bool value)
    {
        _gameSettings.AutomaticReload = value;
    }
}
