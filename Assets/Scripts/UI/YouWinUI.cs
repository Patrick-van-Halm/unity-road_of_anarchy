using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YouWinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;

    private void Start()
    {
        if (NetworkServer.active) _button.interactable = false;
    }

    private void Update()
    {
        Debug.Log(NetworkServer.connections.Count);
        if (NetworkServer.connections.Count == 1) _button.interactable = true;
    }

    public void ShowWinUI(string text)
    {
        _text.text = text;
    }

    public void ShowWinUI(int position, string text)
    {
        string posText = text + position;
        _text.text = posText;
    }

    public void Disconnect()
    {
        GameNetworkManager.Instance?.Disconnect();
    }
}
