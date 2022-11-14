using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PowerupShootDetection : MonoBehaviour, IDamageable
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
