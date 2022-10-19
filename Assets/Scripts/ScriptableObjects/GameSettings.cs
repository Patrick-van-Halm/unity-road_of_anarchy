using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("User settings")]
    public string Username;

    [Header("Audio")]
    public float AudioVolume;

    [Header("Mouse settings")]
    public bool InvertX;
    public bool InvertY;
    public float Sensitivity;

    [Header("Gunner settings")]
    public bool AutomaticReload;
}
