using Mirror;
using UnityEngine;

public class PowerupSpawner : NetworkBehaviour
{
    [SerializeField] private PowerupCollisionDetection _powerupCollisionDetection;
    [SerializeField] private PowerupShootDetection _powerupShootDetection;
    [SerializeField] private GameObject _powerupObj;

    private void Start()
    {
        _powerupCollisionDetection.HasCollided.AddListener(CmdCollectPowerup);
        _powerupShootDetection.HasBeenHit.AddListener(CmdCollectPowerup);
    }

    [Command(requiresAuthority = false)]
    private void CmdCollectPowerup()
    {
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
