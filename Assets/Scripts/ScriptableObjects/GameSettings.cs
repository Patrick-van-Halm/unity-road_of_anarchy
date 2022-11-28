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

    [Header("Player settings")]
    public float FOV;

    [Header("Privacy settings")]
    public bool HideOwnUsername;
    public bool HideOtherUsernames;

    private string _jsonPath => Application.dataPath + "/" + name + ".txt";

    internal void Save()
    {
        string settings = JsonUtility.ToJson(this, true);
        File.WriteAllText(_jsonPath, settings);
    }

    internal void Load()
    {
#if !UNITY_EDITOR
        Reset();
#endif
        if (File.Exists(_jsonPath))
        {
            string settingsText = File.ReadAllText(_jsonPath);
            JsonUtility.FromJsonOverwrite(settingsText, this);
        }
    }

    private void Reset()
    {
        Username = "";
        AudioVolume = .5f;
        InvertX = false;
        InvertY = false;
        Sensitivity = .5f;
        AutomaticReload = false;
        FOV = 50f;
        HideOwnUsername = false;
        HideOtherUsernames = false;
    }
}
