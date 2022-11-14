using Mirror;
using UnityEngine.Events;

public class PowerupShootDetection : NetworkBehaviour, IDamageable
{
    public UnityEvent<Team> HasBeenHit = new UnityEvent<Team>();

    public void ApplyDamage(float value) { }

    [Command(requiresAuthority = false)]
    public void CmdApplyDamage(float value, NetworkConnectionToClient connectionToClient = null)
    {
        TargetHasHitPowerup(connectionToClient);
    }

    private void TargetHasHitPowerup(NetworkConnection target)
    {
        HasBeenHit?.Invoke(target.identity.GetComponent<Player>().Team);
    }
}
