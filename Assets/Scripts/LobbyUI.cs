using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject LobbyPlayerListContent;
    public GameObject LobbyPlayerListRowPrefab;
    public Button StartLobbyButton;
    public Button LeaveLobbyButton;
    public Lobby Lobby;

    private void Start()
    {
        // Listen to lobby events
        Lobby.OnPlayerJoined.AddListener(ReloadLobbyPlayerRows);
        Lobby.OnPlayerLeft.AddListener(ReloadLobbyPlayerRows);
        Lobby.OnLobbyStarted.AddListener(MakeLeaveLobbyButtonNotInteractable);

        // Check if host if not hide the start button
        if (!Lobby.IsLobbyHost) StartLobbyButton.gameObject.SetActive(false);
    }

    private void ReloadLobbyPlayerRows()
    {
        // Destroy all previous players rows
        for (int i = LobbyPlayerListContent.transform.childCount - 1; i > -1; i--)
        {
            Destroy(LobbyPlayerListContent.transform.GetChild(i).gameObject);
        }

        // Create new rows
        foreach (LobbyPlayer player in Lobby.LobbyPlayers)
        {
            GameObject row = Instantiate(LobbyPlayerListRowPrefab, LobbyPlayerListContent.transform);
            LobbyPlayerRowUI listingData = row.GetComponent<LobbyPlayerRowUI>();
            listingData.player = player;
            player.OnReadyStateChanged.AddListener(ReadyStateChanged);
        }
    }

    private void ReadyStateChanged(bool _)
    {
        // Whenever a player changes ready state check if the lobby can start if so then make the start button interactable
        if (Lobby.CanLobbyStart()) StartLobbyButton.interactable = true;
        else StartLobbyButton.interactable = false;
    }

    public void ReadyUp()
    {
        // Mark player as ready
        Lobby.LocalPlayer.SetReady(!Lobby.LocalPlayer.IsReady);
    }

    public void StartLobby()
    {
        // Start the lobby
        Lobby.StartLobby();

        // Disable interaction so the player can't keep spamming it
        StartLobbyButton.interactable = false;
    }

    private void MakeLeaveLobbyButtonNotInteractable()
    {
        LeaveLobbyButton.interactable = false;
    }

    public void LeaveLobby()
    {
        // Check if lobby is already started
        if (Lobby.IsLobbyStarted) return;

        Lobby.Disconnect();
    }
}
