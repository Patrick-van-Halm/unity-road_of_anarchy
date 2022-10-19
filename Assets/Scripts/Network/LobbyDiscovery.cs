using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Mirror;
using System.Net;
using System.Net.Sockets;

public class LobbyDiscovery : MonoBehaviour
{
    public static LobbyDiscovery Instance { get; private set; }
    public UnityEvent OnLobbyListingRefreshed = new UnityEvent();
    public IReadOnlyList<LobbyDetails> Lobbies => _lobbies;

    [SerializeField] private string _discoveryServerAddress = "localhost:3000";
    [SerializeField] private List<LobbyDetails> _lobbies = new List<LobbyDetails>();


    private void Awake()
    {
        // Mark as singleton
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        // Refresh servers on start
        RefreshServers();
    }

    public void RefreshServers()
    {
        // Create a GET request to server
        UnityWebRequest request = UnityWebRequest.Get($"{_discoveryServerAddress}/servers");
        StartCoroutine(request.ProcessRequest(OnServersRefreshed));
    }

    private void OnServersRefreshed(UnityWebRequest request)
    {
        // Store all the servers and invoke event that servers changed
        _lobbies.Clear();
        List<LobbyDetails> lobbies = JsonConvert.DeserializeObject<List<LobbyDetails>>(request.downloadHandler.text);
        if (lobbies == null) return;
        _lobbies.AddRange(lobbies);
        OnLobbyListingRefreshed?.Invoke();
    }
}

[Serializable]
public class LobbyDetails
{
    public string Ip;
    public ushort Port;
}
