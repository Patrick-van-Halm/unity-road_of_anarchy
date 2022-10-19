using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyListingUI : MonoBehaviour
{
    public GameObject LobbyListContent;
    public GameObject LobbyListRowPrefab;

    private void Start()
    {
        // Listen on refreshed event
        LobbyDiscovery.Instance.OnLobbyListingRefreshed.AddListener(ReloadLobbyListingRows);
    }

    public void ReloadLobbyListingRows()
    {
        // Destroy all previous listing rows
        for(int i = LobbyListContent.transform.childCount - 1; i > -1; i--)
        {
            Destroy(LobbyListContent.transform.GetChild(i).gameObject);
        }

        // Create new rows
        foreach(LobbyDetails lobby in LobbyDiscovery.Instance.Lobbies)
        {
            GameObject row = Instantiate(LobbyListRowPrefab, LobbyListContent.transform);
            LobbyListingRowUI listingData = row.GetComponent<LobbyListingRowUI>();
            listingData.Details = lobby;
        }
    }

    public void HostLobby()
    {
        // Host a new lobby
        GameNetworkManager.Instance?.StartHost();
    }
}
