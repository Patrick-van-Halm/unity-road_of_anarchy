using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyListingRowUI : MonoBehaviour
{
    public LobbyDetails Details;

    public void Connect()
    {
        // Connect to lobby using the details
        GameNetworkManager.Instance.networkAddress = Details.Ip;
        GameNetworkManager.Instance.Port = Details.Port;
        GameNetworkManager.Instance.StartClient();
    }
}
