using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HeatBar : MonoBehaviour
{
    [SerializeField] private TMP_Text heatText;
    float maxHeat;

    public void UpdateHeatText(float currentHeat)
    {
        if(currentHeat < maxHeat) heatText.text = $"{Mathf.Round(currentHeat)} / {maxHeat}";
        else heatText.text = $"OVERHEATED";
    }

    public void SetMaxHeat(float heatMaxValue)
    {
        maxHeat = heatMaxValue;
        UpdateHeatText(0);
    }
}
