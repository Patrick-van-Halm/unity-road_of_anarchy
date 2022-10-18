using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetSettings : MonoBehaviour
{
    [Header("UI-elements")]
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Toggle _invertX;
    [SerializeField] private Toggle _invertY;
    [SerializeField] private Slider _sensitivity;

    [Header("Settings")]
    [SerializeField] private GameSettings _gameSettings;
    void Start()
    {
        _nameInput.text = _gameSettings.Username;
        _volumeSlider.value = _gameSettings.AudioVolume;
        _invertX.isOn = _gameSettings.InvertX;
        _invertY.isOn = _gameSettings.InvertY;
        _sensitivity.value = _gameSettings.Sensitivity;
    }

}
