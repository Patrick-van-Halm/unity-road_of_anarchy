using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar] public Team Team;
    [SyncVar] public string name = "Johnson";
    private PlayerHUDComponent _hudComponent;

    private void Start()
    {
        _hudComponent = FindObjectOfType<PlayerHUDComponent>();
    }

    [TargetRpc]
    public void TargetOnTeamKill(NetworkConnection conn)
    {
        _hudComponent?.OnKillFeedMessage("Your team has killed another team!");
    }

    [TargetRpc]
    public void TargetOnEliminated(NetworkConnection conn)
    {
        _hudComponent?.ShowEliminatedUI();
    }
}

public class Team
{
    public Gunner Gunner => GunnerIdentity.GetComponent<Gunner>();
    public Player GunnerPlayer => GunnerIdentity.GetComponent<Player>();
    public Spectator GunnerSpectator => GunnerIdentity.GetComponent<Spectator>();
    public NetworkIdentity GunnerIdentity;

    public Vehicle Vehicle => DriverIdentity.GetComponent<Vehicle>();
    public Player DriverPlayer => GunnerIdentity.GetComponent<Player>();
    public Spectator DriverSpectator => DriverIdentity.GetComponent<Spectator>();
    public NetworkIdentity DriverIdentity;
}
