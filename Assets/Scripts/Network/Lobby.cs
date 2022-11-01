using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Lobby : NetworkBehaviour
{
    public GameObject LobbyPlayerPrefab;

    public IReadOnlyList<LobbyPlayer> LobbyPlayers => FindObjectsOfType<LobbyPlayer>();
    [Scene] public List<string> GameScenes = new List<string>();

    public LobbyPlayer LocalPlayer => NetworkClient.localPlayer.GetComponent<LobbyPlayer>();
    public bool IsLobbyHost => NetworkServer.active;

    private Coroutine _passiveLobbyStartCountdown;
    private Coroutine _lobbyStartCountdown;
    [SerializeField] private int _minPlayersRequired = 4;
    [SerializeField] private int _passiveCountdownSeconds = 60;
    [SerializeField] private int _lobbyStartCountdownSeconds = 5;

    public UnityEvent OnPlayerJoined = new UnityEvent();
    public UnityEvent OnPlayerLeft = new UnityEvent();
    public UnityEvent OnLobbyStarted = new UnityEvent();
    public UnityEvent<int> OnCountdownValueChanged = new UnityEvent<int>();

    [SyncVar(hook = nameof(OnIsLobbyStartedChanged))] public bool IsLobbyStarted;
    [SyncVar(hook = nameof(OnLobbyCountdownSecondsChanged))] public int lobbyCountdownSeconds;

    private void Awake()
    {
        if (!NetworkServer.active) return;
        // Listen to networking events
        GameNetworkManager.Instance.OnServerAddPlayerToClient.AddListener(ReplacePlayerAsLobbyPlayer);
        GameNetworkManager.Instance.OnClientDisconnectedFromServer.AddListener(OnPlayerDisconnected);

        // Allow new connections
        GameNetworkManager.Instance.AcceptNewConnections = true;
    }

    [Server]
    private void OnPlayerDisconnected(NetworkConnectionToClient player)
    {
        // Call player left event
        RpcPlayerLeft();

        // Check if player amount is less than minimum
        if (LobbyPlayers.Count >= _minPlayersRequired) return;

        // Check if player amount is enough to fill all cars and gunners
        if(LobbyPlayers.Count % 2 == 0) return;

        // Stop start lobby
        if(_lobbyStartCountdown != null)
        {
            StopCoroutine(_lobbyStartCountdown);
            _lobbyStartCountdown = null;
            _lobbyStartCountdownSeconds = 5;
            lobbyCountdownSeconds = 60;
            GameNetworkManager.Instance.AcceptNewConnections = true;
        }

        // Stop passive countdown coroutine
        if (_passiveLobbyStartCountdown == null)
        {
            StopCoroutine(_passiveLobbyStartCountdown);
            _passiveLobbyStartCountdown = null;
            _passiveCountdownSeconds = 60;
            lobbyCountdownSeconds = 60;
        }
    }

    [ClientRpc]
    private void RpcPlayerLeft()
    {
        OnPlayerLeft.Invoke();
    }

    public bool StartLobby(bool force = false)
    {
        // Check if minimum amount of players is 4
        if (LobbyPlayers.Count < _minPlayersRequired) return false;

        // Check if all players are ready
        if(!force && !LobbyPlayers.All(p => p.IsReady)) return false;

        // Check if Game scene length is bigger than 0
        if(GameScenes.Count == 0) return false;

        // Start lobby countdown
        _lobbyStartCountdown = StartCoroutine(CoroStartLobby());

        // Stop the passive countdown and reset it to null
        StopCoroutine(_passiveLobbyStartCountdown);
        _passiveLobbyStartCountdown = null;
        _passiveCountdownSeconds = 60;
        lobbyCountdownSeconds = 60;

        IsLobbyStarted = true;

        // Return true cause lobby is started
        return true;
    }

    private void OnIsLobbyStartedChanged(bool oldValue, bool newValue)
    {
        OnLobbyStarted?.Invoke();
    }

    private void OnLobbyCountdownSecondsChanged(int oldValue, int newValue)
    {
        OnCountdownValueChanged?.Invoke(newValue);
    }

    private IEnumerator CoroStartLobby()
    {
        // Disable new connections so no one new can connect
        GameNetworkManager.Instance.AcceptNewConnections = false;

        int timePassed = 0;
        lobbyCountdownSeconds = _lobbyStartCountdownSeconds;
        while (timePassed < _lobbyStartCountdownSeconds)
        {
            // Wait for the countdown
            yield return new WaitForSeconds(1);
            timePassed++;
            lobbyCountdownSeconds--;
        }

        // Change scene to random Game scene
        GameNetworkManager.Instance?.ServerChangeScene(GameScenes.Random());

        // Unlist lobby since it has started
        GameNetworkManager.Instance?.RemoveLobbyFromRegistry();
    }

    [Server]
    public void ReplacePlayerAsLobbyPlayer(NetworkConnectionToClient player)
    {
        // Get current player object
        GameObject curPlayer = player.identity.gameObject;

        // Create new lobby player
        GameObject lobbyPlayer = CreateLobbyPlayer();

        // Rename lobby player to easily identify player
        lobbyPlayer.name = $"LobbyPlayer ({player.connectionId})";

        // Replace current player object with lobby player for everyone connected
        NetworkServer.ReplacePlayerForConnection(player, lobbyPlayer);

        // Destroy old player object after .1 second
        Destroy(curPlayer, .1f);

        // Call player joined event
        RpcPlayerJoined();
    }

    [ClientRpc]
    private void RpcPlayerJoined()
    {
        OnPlayerJoined?.Invoke();
    }

    private GameObject CreateLobbyPlayer()
    {
        // Spawn lobby player prefab
        GameObject lobbyPlayer = Instantiate(LobbyPlayerPrefab);

        // Check if player amount is 4 or more and start passive countdown if not started yet
        CheckEnoughPlayersToStart();

        // Return lobby player
        return lobbyPlayer;
    }

    private void CheckEnoughPlayersToStart()
    {
        // Unset passive coroutine if set
        if (_passiveLobbyStartCountdown != null)
        {
            StopCoroutine(_passiveLobbyStartCountdown);
            _passiveLobbyStartCountdown = null;
            _passiveCountdownSeconds = 60;
            lobbyCountdownSeconds = 60;
        }

        // Check if player amount is 4 or more
        if (LobbyPlayers.Count < _minPlayersRequired) return;

        // Check if enough players to fill all roles
        if (LobbyPlayers.Count % 2 != 0) return;

        // Start passive countdown
        _passiveLobbyStartCountdown = StartCoroutine(CoroPassiveLobbyStartCountdown());
    }

    public bool CanLobbyStart()
    {
        // Check if lobby has min players
        if(LobbyPlayers.Count < _minPlayersRequired) return false;

        // Check if enough players to fill all roles
        if (LobbyPlayers.Count % 2 != 0) return false;

        // Check if everyone ready
        if (!LobbyPlayers.All(p => p.IsReady)) return false;

        // Check if there are game scenes registered
        if(GameScenes.Count == 0) return false;

        // Return true
        return true;
    }

    private IEnumerator CoroPassiveLobbyStartCountdown()
    {
        int timePassed = 0;
        lobbyCountdownSeconds = _passiveCountdownSeconds;
        while (timePassed < _passiveCountdownSeconds)
        {
            // Wait for the specified amount of time and then start the lobby
            yield return new WaitForSeconds(1);
            timePassed++;
            lobbyCountdownSeconds--;
        }
        StartLobby(true);
    }

    public void Disconnect()
    {
        GameNetworkManager.Instance?.Disconnect();
    }
}
