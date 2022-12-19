using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGuidedMissile : NetworkBehaviour
{
    [SerializeField] private GuidedMissile _guidedMissileScriptableObject;
    [SerializeField] private WeaponManager _weaponManager;

    public void PlayerFiredMissile()
    {
        CmdFireMissile();
    }

    [Command]
    private void CmdFireMissile(NetworkConnectionToClient sender = null)
    {
        // Get target transform
        GameObject car = GetComponent<Gunner>().Team.Vehicle.gameObject;
        int position = RaceManager.Instance.GetCarPosition(car);
        Transform target = RaceManager.Instance.GetCarTransform(position - 1);

        // Spawn Missile
        GameObject missile = Instantiate(_guidedMissileScriptableObject.MissilePrefab, transform.position, Quaternion.identity);

        // Rotate missile towards gunner camera
        missile.transform.forward = GetComponentInChildren<Camera>().transform.forward;

        // Get missile movement script
        GuidedMissileMovement guidedMissileMovementScript = missile.GetComponent<GuidedMissileMovement>();
        guidedMissileMovementScript.SetValues(target, car);

        NetworkServer.Spawn(missile, sender);
    }

    public void ActivateMissile()
    {
        // Give reference to this script to weaponManager
        _weaponManager.SetActivateGuidedMissile(this);
        _weaponManager.FireMissile = true;
    }
}