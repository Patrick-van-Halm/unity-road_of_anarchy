using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAudioSync : NetworkBehaviour
{
    #region Sync Gun Moving
    public bool IsMoving => _isMoving;
    [SyncVar, SerializeField] private bool _isMoving;

    [Command(requiresAuthority = true)]
    public void SetIsMoving(bool moving)
    {
        _isMoving = moving;
    }
    #endregion
}
