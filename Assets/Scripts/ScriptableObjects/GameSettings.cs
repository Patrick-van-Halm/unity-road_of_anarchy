using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private string _jsonPath => Application.dataPath + "/" + name + ".txt";

    internal void Save()
    {
        string settings = JsonUtility.ToJson(this, true);
        File.WriteAllText(_jsonPath, settings);
    }

    internal void Load()
    {
        if (File.Exists(_jsonPath))
        {
            string settingsText = File.ReadAllText(_jsonPath);
            JsonUtility.FromJsonOverwrite(settingsText, this);
        }
    }

}
