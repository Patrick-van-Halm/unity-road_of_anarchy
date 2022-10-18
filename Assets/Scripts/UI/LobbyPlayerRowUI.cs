using UnityEngine;
using TMPro;
using System;

public class LobbyPlayerRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameElement;
    public LobbyPlayer player;

    private void Start()
    {
        // Set the name to red by default
        _nameElement.color = Color.red;
        
        // Listen to player events
        player.OnReadyStateChanged.AddListener(ReadyStateChanged);

        // Set the name
        SetName(player.Name);
        player.OnPlayerNameChanged.AddListener(SetName);
    }

    private void ReadyStateChanged(bool isReady)
    {
        // Change the name color based of ready state
        _nameElement.color = !isReady ? Color.red : Color.green;
    }

    private void SetName(string name)
    {
        // Change the name
        _nameElement.text = name;
    }
}
