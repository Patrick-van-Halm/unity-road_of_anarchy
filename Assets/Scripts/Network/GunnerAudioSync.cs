using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunnerAudioSync : NetworkBehaviour
{
    #region Sync Gun Moving
    public bool IsMoving => _isMoving;
    [SyncVar(hook = nameof(SetIsMovingChanged)), SerializeField] private bool _isMoving;
    public UnityEvent<bool> OnIsMovingChanged = new();

    [Command(requiresAuthority = true)]
    public void SetIsMoving(bool moving)
    {
        _isMoving = moving;
    }

    private void SetIsMovingChanged(bool oldValue, bool newValue)
    {
        OnIsMovingChanged?.Invoke(newValue);
    }
    #endregion
}
