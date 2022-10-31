using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : NetworkBehaviour
{
    [SerializeField] GameObject _ammoBox;
    [SerializeField] Weapon _weapon;

    private int _ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Get the vehicle interacting
        Vehicle vehicle = other.GetComponent<Vehicle>();
        if(vehicle == null) return;

        // Get the drivers vehicle (Only the driver may interact)
        Vehicle myVehicle = NetworkClient.connection.identity.GetComponent<Vehicle>();
        if(myVehicle == null) return;

        // Replacing with logic that will work on the network cause this logic needs to run on the server
        //if (other.gameObject.tag == "Vehicle")
        //{
        //    AddAmmo();
        //    StartCoroutine(RespawnAmmo());
        //}

        // Check if the team is the same cause you can't replenish ammo for different teams
        if (vehicle.Team != myVehicle.Team) return;

        // Execute logic to pickup ammo
        CmdPickupAmmo();
    }

    [Command(requiresAuthority = false)]
    private void CmdPickupAmmo(NetworkConnectionToClient sender = null)
    {
        // Exit this if the ammo box is inactive in scene meaning its picked up
        if (!_ammoBox.activeSelf) return;

        // Set ammo box inactive
        _ammoBox.SetActive(false);

        // Hide the box on all clients
        RpcHideBox();

        // Send add ammo to gunner of person that picked up the box
        TargetAddAmmo(sender.identity.GetComponent<Vehicle>().Team.GunnerIdentity.connectionToClient, _ammoAmount);

        // Start timer to make ammo box active again
        StartCoroutine(RespawnAmmo());
    }

    /// <summary>
    /// Adds ammo and destroys ammo pickup
    /// </summary>
    [TargetRpc]
    private void TargetAddAmmo(NetworkConnection target, int ammo)
    {
        // Make Target RPC of this function so it can be send to the gunner with the intended ammo. So ammo can't be changed on the client (Cheat mitigation)
        // Move logic to Client RPC and server
        //_ammoBox.SetActive(false);

        // Code for adding ammo
        _weapon.AmmoAmount += ammo;
        if (_weapon.AmmoAmount > _weapon.MaxAmmo) _weapon.AmmoAmount = _weapon.MaxAmmo;
    }

    [ClientRpc]
    private void RpcHideBox()
    {
        _ammoBox.SetActive(false);
    }

    [ClientRpc]
    private void RpcShowBox()
    {
        _ammoBox.SetActive(true);
    }

    /// <summary>
    /// Respawns ammo after amount of seconds
    /// </summary>
    IEnumerator RespawnAmmo()
    {
        yield return new WaitForSeconds(3f);

        // Show the box on server and all clients
        _ammoBox.SetActive(true);
        RpcShowBox();
    }
}
