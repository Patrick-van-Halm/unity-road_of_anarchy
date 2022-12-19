using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyListingRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _lobbyNameTbx;
    [SerializeField] private TMP_Text _lobbyPlayerCountTbx;
    public LobbyDetails Details;

    private void Start()
    {
        _lobbyNameTbx.text = Details.Name;
        _lobbyPlayerCountTbx.text = Details.PlayerCount.ToString();
    }

    public void Connect()
    {
        // Connect to lobby using the details
        GameNetworkManager.Instance.CurrentLobbyDetails = Details;
        GameNetworkManager.Instance.networkAddress = Details.Ip;
        GameNetworkManager.Instance.Port = Details.Port;
        GameNetworkManager.Instance.StartClient();
    }
}
