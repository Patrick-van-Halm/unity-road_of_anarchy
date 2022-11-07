using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

[System.Serializable]
[CreateAssetMenu(menuName = nameof(Weapon))]
public class Weapon : ScriptableObject
{
    [Tooltip("Sets the name of the weapon that will be displayed in the player UI.")]
    public string Name = "DefaultWeapon";

    [Header("Weapon Audio")]
    [Tooltip("The sound of the weapon when firing")]
    public EventReference _fireSoundRef;

    [Tooltip("The sound played when a hit is detected")]
    public EventReference _hitSoundRef;

    [Header("Weapon Properties")]
    [Tooltip("Damage value of the weapon.")]
    public float DamageAmount = 10f;

    [Tooltip("Amount of ammo the player starts with.")]
    public int AmmoAmount = 0;

    [Tooltip("The maximum amount of ammo the weapon can hold.")]
    public int MaxAmmo = 50;

    [Tooltip("The amount of times the weapon can fire, before the player has to reload.")]
    public int MaxClipSize = 10;

    [Tooltip("Amount of ammo in a single clip. If this is empty, the player has to reload.")]
    public int ClipAmmoAmount = 0;

    [Tooltip("The range the weapon can shoot")]
    public float WeaponRange = 200f;

    [Header("Weapon Timing")]
    [Tooltip("Time it takes for the weapon to reload.")]
    public float ReloadTimeInSeconds = 2f;

    [Tooltip("FireRate of the weapon when the player holds Mouse1")]
    public float FireRateDelayInSeconds = 0.2f;
}
