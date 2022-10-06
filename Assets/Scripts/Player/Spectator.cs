using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : Player
{
    [SyncVar] public bool wasEliminated;
    private PlayerHUDComponent _hudComponent;
    private bool spawnedEliminatedUI;

    private void Start()
    {
        _hudComponent = FindObjectOfType<PlayerHUDComponent>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if(wasEliminated && !spawnedEliminatedUI)
        {
            spawnedEliminatedUI = true;
            _hudComponent.ShowEliminatedUI();
        }
    }
}
