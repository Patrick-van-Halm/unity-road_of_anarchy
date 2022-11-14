using Mirror;
using UnityEngine;

public class PowerupSpawner : NetworkBehaviour
{
    [SerializeField] private PowerupCollisionDetection _powerupCollisionDetection;
    [SerializeField] private PowerupShootDetection _powerupShootDetection;
    [SerializeField] private GameObject _powerupObj;

    private void Start()
    {
        if (_powerupCollisionDetection != null) _powerupCollisionDetection.HasCollided.AddListener(CmdCollectPowerup);
        if (_powerupShootDetection != null) _powerupShootDetection.HasBeenHit.AddListener(CmdCollectPowerup);
    }

    [Command(requiresAuthority = false)]
    private void CmdCollectPowerup(Team team)
    {
        PowerupManager.Instance?.Pickup(team, this);
        RpcCollectPowerup();
    }

    [ClientRpc]
    private void RpcCollectPowerup()
    {
        _powerupObj.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    public void CmdRespawnPowerup()
    {
        RpcRespawnPowerup();
    }

    [ClientRpc]
    private void RpcRespawnPowerup()
    {
        _powerupObj.SetActive(true);
    }
}
