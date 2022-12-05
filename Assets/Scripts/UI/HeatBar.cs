using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _heatText;
    [SerializeField] private RectTransform _heatTbxRectTransform;
    [SerializeField] private Slider _heatSlider;
    private float _maxHeat;
    private float _heatTbxMinSize = 25f;
    private float _heatTbxMaxSize = 137.5f;
    private float _heatTbxHeight;

    private void Start()
    {
        _heatTbxHeight = _heatTbxRectTransform.rect.height;
    }

    public void UpdateHeatText(float currentHeat)
    {
        if(currentHeat < _maxHeat)
        {
            _heatTbxRectTransform.sizeDelta = new Vector2(_heatTbxMinSize, _heatTbxHeight);
            _heatText.text = $"{Mathf.Round(currentHeat)}";
            _heatSlider.gameObject.SetActive(true);
            _heatSlider.value = currentHeat;
        }
        else
        {
            _heatTbxRectTransform.sizeDelta = new Vector2(_heatTbxMaxSize, _heatTbxHeight);
            _heatText.text = $"OVERHEATED";
            _heatSlider.value = _maxHeat;
            _heatSlider.gameObject.SetActive(false);
        }
    }

    public void SetMaxHeat(float heatMaxValue)
    {
        _maxHeat = heatMaxValue;
        _heatSlider.maxValue = _maxHeat;
        _heatSlider.value = 0;
        UpdateHeatText(0);
    }
}
