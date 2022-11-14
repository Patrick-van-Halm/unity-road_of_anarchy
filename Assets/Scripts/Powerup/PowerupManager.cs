using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupManager : NetworkBehaviour
{
    [SerializeField] private List<BasePowerup> _powerups;
    [SerializeField] private float _respawnTime;

    public static PowerupManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    [Server]
    public void Pickup(Team team, PowerupSpawner powerupSpawner)
    {
        List<BasePowerup> filteredPowerups = _powerups.Where(p => p.MinimumPosition <= RaceManager.Instance.GetCarPosition(team.DriverIdentity.gameObject)).ToList();
        int powerupIndex = Random.Range(0, filteredPowerups.Count);
        BasePowerup powerup = filteredPowerups[powerupIndex];
        StartCoroutine(CoroRespawnPowerup(powerupSpawner));

        if (powerup.TargetTeamMember == PlayerRole.Driver)
            TargetPickupPowerup(team.DriverIdentity.connectionToClient, _powerups.IndexOf(powerup));
        else
            TargetPickupPowerup(team.GunnerIdentity.connectionToClient, _powerups.IndexOf(powerup));
    }

    [TargetRpc]
    private void TargetPickupPowerup(NetworkConnection target, int powerupIndex)
    {
        _powerups[powerupIndex].Pickup();
    }

    private IEnumerator CoroRespawnPowerup(PowerupSpawner powerupSpawner)
    {
        yield return new WaitForSeconds(_respawnTime);
        powerupSpawner.CmdRespawnPowerup();
    }
}
