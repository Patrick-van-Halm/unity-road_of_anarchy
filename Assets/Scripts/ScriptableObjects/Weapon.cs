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
    [Header("Tags")]
    [Tooltip("Sets the name of the weapon that will be displayed in the player UI.")]
    public string Name = "DefaultWeapon";


    [Header("Weapon Audio")]
    [Tooltip("The sound of the weapon when firing")]
    public EventReference FireSoundRef;

    [Tooltip("The sound played when a hit is detected")]
    public EventReference HitSoundRef;

    [Tooltip("The sound played when a the weapon is overheated")]
    public EventReference OverheatSoundRef;

    [Tooltip("The sound played when player tries to fire and there is no ammo in clip")]
    public EventReference NoAmmoSoundRef;


    [Header("Weapon")]
    [Tooltip("The state the weapon is in on init")]
    public WeaponState WeaponState = WeaponState.ReadyToShoot;

    [Tooltip("Damage value of the weapon.")]
    public float DamageAmount = 15f;

    [Tooltip("The range the weapon can shoot")]
    public float WeaponRange = 200f;


    [Header("Ammo")]
    [Tooltip("Amount of ammo the player starts with.")]
    public int AmmoAmount = 0;

    [Tooltip("The maximum amount of ammo the weapon can hold.")]
    public int MaxAmmo = 50;

    [Tooltip("The amount of times the weapon can fire, before the player has to reload.")]
    public int MaxClipSize = 15;

    [Tooltip("Amount of ammo in a single clip. If this is empty, the player has to reload.")]
    public int ClipAmmoAmount = 0;


    [Header("Timing (in seconds)")]
    [Tooltip("Time it takes for the weapon to reload.")]
    public float ReloadTime = 2f;

    [Tooltip("FireRate of the weapon when the player holds Mouse1")]
    public float FireRateDelay = 0.2f;

    [Tooltip("The amount of seconds the gunner has to wait once the weapon has the overheating state")]
    public float OverheatedTime = 1.0f;

    [Tooltip("The time it takes before the gun is cooled by one tick")]
    public float CooldownTime = 0.8f;


    [Header("Overheating")]
    [Tooltip("The maximum heat value before the weapon goes into the overheating state")]
    public float HeatMaxValue = 40.0f;

    [Tooltip("The maximum heat value before the weapon goes into the overheating state")]
    public float CurrentHeatValue = 0.0f;

    [Tooltip("The amount this weapon adds to the total heath counter")]
    public float HeatPerShotValue = 4.0f;

    [Tooltip("The amount of heat that is subtracted from the current heat value")]
    public float HeatReducingValue = 4f;

    [Tooltip("The amount of heat that is subtracted from the current heat value when in water")]
    public float HeatReducingValueInWater = 10f;
}
