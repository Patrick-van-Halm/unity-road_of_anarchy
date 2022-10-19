using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PillarShootDetection : MonoBehaviour, IDamageable
{
    public UnityEvent HasBeenHit = new UnityEvent();

    public void ApplyDamage(float value)
    {
        HasBeenHit?.Invoke();
    }

    public void CmdApplyDamage(float value, NetworkConnectionToClient connectionToClient = null)
    {
        ApplyDamage(0);
    }
}
