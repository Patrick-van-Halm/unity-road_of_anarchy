using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameNetworkManager : NetworkManager
{
    [Header("Scenes")]
    [Scene] public string FallbackScene;

    [Header("Events")]
    public UnityEvent<NetworkConnectionToClient> OnClientConnectedToServer;
    public UnityEvent<NetworkConnectionToClient> OnServerAddPlayerToClient;
    public UnityEvent<NetworkConnectionToClient> OnClientIsReady;
    public UnityEvent<NetworkConnectionToClient> OnClientDisconnectedFromServer;
    public UnityEvent OnClientDisconnected;
    public UnityEvent OnServerStarted;
    public UnityEvent OnServerClose;

    [Header("Server settings")]
    public bool UseLan;
    public bool AcceptNewConnections;

    [HideInInspector] public LobbyDetails CurrentLobbyDetails;
    public string DiscoveryServerAddress => "http://84.26.114.127:5555";
    public string OverwriteIP = "";


    public static GameNetworkManager Instance { get; private set; }

    public override void Awake()
    {
        base.Awake();
        if(Instance == null) Instance = this;
    }

    public ushort Port
    {
        get
        {
            if (this.transport is KcpTransport) return (this.transport as KcpTransport).Port;
            return 0;
        }
        set
        {
            if (this.transport is KcpTransport) (this.transport as KcpTransport).Port = value;
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!AcceptNewConnections)
        {
            conn.Disconnect();
            return;
        }
        base.OnServerConnect(conn);
        OnClientConnectedToServer?.Invoke(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        OnServerAddPlayerToClient?.Invoke(conn);
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        OnClientIsReady?.Invoke(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene(FallbackScene);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        OnClientDisconnectedFromServer?.Invoke(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        RegisterLobby();
        OnServerStarted?.Invoke();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        RemoveLobbyFromRegistry();
        OnServerClose?.Invoke();
    }

    public void Disconnect()
    {
        if (NetworkServer.active && NetworkClient.active) StopHost();
        else if(NetworkClient.active) StopClient();
        else if(NetworkServer.active) StopServer();
    }

    override public void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        StopHost();
    }

    public override void OnClientError(Exception exception)
    {
        base.OnClientError(exception);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        if (NetworkServer.active) return;
        CurrentLobbyDetails = new LobbyDetails() { Ip = networkAddress, Port = Port};
    }

    public void RegisterLobby()
    {
        if (UseLan)
        {
            RegisterLobby(new LobbyDetails() { Ip = OverwriteIP == "" ? GetLocalIPAddress() : OverwriteIP, Port = Port });
            return;
        }

        // Create a GET request to get the current external IP
        UnityWebRequest request = UnityWebRequest.Get($"https://api.ipify.org/");
        StartCoroutine(request.ProcessRequest(OnPublicIpReceived));
    }

    private void OnPublicIpReceived(UnityWebRequest data)
    {
        // When the external IP was received create a POST request for registering the lobby
        LobbyDetails details = new LobbyDetails() { Ip = data.downloadHandler.text, Port = Port };
        RegisterLobby(details);
    }

    private void RegisterLobby(LobbyDetails details)
    {
        CurrentLobbyDetails = details;

        // Register the lobby on server
        WWWForm postData = new WWWForm();
        postData.AddField("ip", details.Ip);
        postData.AddField("port", details.Port);
        UnityWebRequest request = UnityWebRequest.Post($"{DiscoveryServerAddress}/servers", postData);
        StartCoroutine(request.ProcessRequest());
    }

    public void RemoveLobbyFromRegistry()
    {
        // Remove server from registry
        UnityWebRequest request = UnityWebRequest.Delete($"{DiscoveryServerAddress}/servers?ip={CurrentLobbyDetails.Ip}&port={CurrentLobbyDetails.Port}");
        StartCoroutine(request.ProcessRequest());
    }

    // Gathered from stackoverflow https://stackoverflow.com/questions/6803073/get-local-ip-address
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
