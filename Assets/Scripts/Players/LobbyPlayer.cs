using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar] public string Name;
    [SyncVar(hook = nameof(OnIsReadyChanged))] public bool IsReady;

    public UnityEvent<bool> OnReadyStateChanged;

    private void OnIsReadyChanged(bool _, bool newValue)
    {
        // Invoke ready state change event
        OnReadyStateChanged.Invoke(newValue);
    }

    [Command(requiresAuthority = true)]
    public void SetReady(bool ready)
    {
        IsReady = ready;
    }
}
