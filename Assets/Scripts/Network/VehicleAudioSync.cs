using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAudioSync : NetworkBehaviour
{
    #region Sync Speed
    public float Speed => _speed;
    [SyncVar, SerializeField] private float _speed;


    [Command(requiresAuthority = true)]
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    #endregion

    #region Sync Braking
    public bool IsBraking => _isBraking;
    [SyncVar, SerializeField] private bool _isBraking;

    public bool IsMoving => _isMoving;
    [SyncVar, SerializeField] private bool _isMoving;


    [Command(requiresAuthority = true)]
    public void SetIsBraking(bool braking)
    {
        _isBraking = braking;
    }

    [Command(requiresAuthority = true)]
    public void SetIsMoving(bool moving)
    {
        _isMoving = moving;
    }
    #endregion
}
