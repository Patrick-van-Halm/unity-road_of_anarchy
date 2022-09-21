using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance { get; private set; }
    [SerializeField] private GameObject respawnScreen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void ShowRespawnScreen()
    {
        respawnScreen.SetActive(true);
    }

    public void HideRespawnScreen()
    {
        respawnScreen.SetActive(false);
    }
}
