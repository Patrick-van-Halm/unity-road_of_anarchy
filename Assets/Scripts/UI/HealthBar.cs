using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _criticalHealthValue;
    [SerializeField] private Color _criticalHealthColor;
    private TMP_Text _text;

    private void Awake()
    {
        _criticalHealthValue = 30f;
        _criticalHealthColor = Color.red;
        _text = GetComponentInChildren<TMP_Text>();
    }

    public void SetHealthText(float newValue)
    {
        _text.text = newValue.ToString();

        if (newValue <= _criticalHealthValue)
            _text.color = _criticalHealthColor;
    }
}
