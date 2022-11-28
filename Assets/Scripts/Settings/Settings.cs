using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI-elements")]
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Toggle _invertX;
    [SerializeField] private Toggle _invertY;
    [SerializeField] private Slider _sensitivity;
    [SerializeField] private Toggle _automaticReload;
    [SerializeField] private Slider _fov;
    [SerializeField] private Toggle _hideYourUsername;
    [SerializeField] private Toggle _hideOtherUsernames;

    [Header("Settings")]
    [SerializeField] private GameSettings _gameSettings;
    private AudioSettings _audioSettings;

    private void Awake()
    {
        _audioSettings = GetComponent<AudioSettings>();
    }

    void Start()
    {
        _gameSettings.Load();

        _nameInput.text = _gameSettings.Username;
        _volumeSlider.value = _gameSettings.AudioVolume;
        _fov.value = _gameSettings.FOV;
        _invertX.isOn = _gameSettings.InvertX;
        _invertY.isOn = _gameSettings.InvertY;
        _sensitivity.value = _gameSettings.Sensitivity;
        _automaticReload.isOn = _gameSettings.AutomaticReload;
        _hideYourUsername.isOn = _gameSettings.HideOwnUsername;
        _hideOtherUsernames.isOn = _gameSettings.HideOtherUsernames;

        _audioSettings.ChangeMasterVolume(_gameSettings.AudioVolume);
    }

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

    public void SetFOV(float value)
    {
        _gameSettings.FOV = value;
    }

    public void SetHideOwnUsername(bool value)
    {
        _gameSettings.HideOwnUsername = value;
    }

    public void SetHideOtherUsernames(bool value)
    {
        _gameSettings.HideOtherUsernames = value;
    }

    public void Save()
    {
        _gameSettings.Save();
    }

}
