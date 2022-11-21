using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class LobbyUI : MonoBehaviour
{
    public GameObject LobbyPlayerListContent;
    public GameObject LobbyPlayerListRowPrefab;
    public Button StartLobbyButton;
    public TMP_Text CountdownTextElement;
    public Button LeaveLobbyButton;
    public Lobby Lobby;

    private EventInstance _countdownAudioEvent;
    private EventInstance _readyAudioEvent;

    private bool _driverOnly = true;
    public Button DriverRoleButton;
    private bool _gunnerOnly = true;
    public Button GunnerRoleButton;
    [Header("Active role button color")]
    public ColorBlock ActiveRoleColors;
    [Header("Inactive role button color")]
    public ColorBlock InactiveRoleColors;

    private void Awake()
    {
        _countdownAudioEvent = RuntimeManager.CreateInstance("event:/CountdownSfx");
        _readyAudioEvent = RuntimeManager.CreateInstance("event:/ReadySfx");
    }

    private void Start()
    {
        // Listen to lobby events
        Lobby.OnPlayerJoined.AddListener(ReloadLobbyPlayerRows);
        Lobby.OnPlayerLeft.AddListener(ReloadLobbyPlayerRows);
        Lobby.OnLobbyStarted.AddListener(MakeLeaveLobbyButtonNotInteractable);
        Lobby.OnCountdownValueChanged.AddListener(UpdateCountdownText);
        Lobby.OnCountdownValueChanged.AddListener(UpdateCountdownAudioEventParameter);

        // Check if host if not hide the start button
        if (!Lobby.IsLobbyHost) StartLobbyButton.gameObject.SetActive(false);

        // Start countdown audio event
        _countdownAudioEvent.start();

        // Update the button colors
        DriverRoleButton.colors = ActiveRoleColors;
        GunnerRoleButton.colors = ActiveRoleColors;
    }

    private void UpdateCountdownAudioEventParameter(int value)
    {
        _countdownAudioEvent.setParameterByName("secondsLeft", value);
    }

    private void UpdateCountdownText(int value)
    {
        CountdownTextElement.color = Color.black;
        if (!Lobby.IsLobbyStarted)
        {
            if (value <= 5) CountdownTextElement.color = Color.red;
            CountdownTextElement.text = $"Starting in: {value}";
        }
        else CountdownTextElement.text = $"GET READY!";
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
            player.OnReadyStateChanged.AddListener(PlayReadySoundWhenReady);
        }
    }

    private void PlayReadySoundWhenReady(bool isReady)
    {
        if(isReady) _readyAudioEvent.start();
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

    private void SetPreferenceRole()
    {
        if (_driverOnly && _gunnerOnly)
            Lobby.LocalPlayer.SetRolePreference(LobbyPlayer.RolesPreference.Both);
        else if (_gunnerOnly)
            Lobby.LocalPlayer.SetRolePreference(LobbyPlayer.RolesPreference.OnlyGunner);
        else if (_driverOnly)
            Lobby.LocalPlayer.SetRolePreference(LobbyPlayer.RolesPreference.OnlyDriver);
    }

    public void SelectDriverRole()
    {
        if (!_gunnerOnly) return;
        _driverOnly = !_driverOnly;
        DriverRoleButton.colors = _driverOnly ? ActiveRoleColors : InactiveRoleColors;
        SetPreferenceRole();
    }

    public void SelectGunnerRole()
    {
        if (!_driverOnly) return;
        _gunnerOnly = !_gunnerOnly;
        GunnerRoleButton.colors = _gunnerOnly ? ActiveRoleColors : InactiveRoleColors;
        SetPreferenceRole();
    }
}
