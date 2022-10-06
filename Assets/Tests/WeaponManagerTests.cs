using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeaponManagerTests
{
    private GameObject GameObject {get; set;} 

    [SetUp]
    public void InitTests()
    {
        GameObject = new GameObject(nameof(WeaponManagerTests));
        GameObject.AddComponent<PlayerHUDComponent>();
        Gunner player = GameObject.AddComponent<Gunner>();
        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        weaponManager.Weapon = ScriptableObject.CreateInstance<Weapon>();
    }

    [UnityTest]
    public IEnumerator WhenReloadingWeaponShouldResetClip()
    {
        int expectedClipAmount = 5;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        
        // Clip is currently empty, and can only hold 5 bullets.
        weaponManager.Weapon.MaxAmmo = 30;
        weaponManager.Weapon.AmmoAmount = 5;

        weaponManager.Weapon.MaxClipSize = 5;
        weaponManager.Weapon.ClipAmmoAmount = 0;

        weaponManager.ReloadWeapon();

        // Wait untill reload has finished
        yield return new WaitForSeconds(4);

        Assert.AreEqual(expectedClipAmount, weaponManager.Weapon.ClipAmmoAmount);
    }

    [UnityTest]
    public IEnumerator WhenReloadingWithoutAmmoLeftShouldDoNothing()
    {
        int expectedClipAmount = 0;
        int expectedAmmoAmount = 0;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        
        weaponManager.Weapon.MaxAmmo = 30;
        weaponManager.Weapon.AmmoAmount = 0;
        weaponManager.Weapon.MaxClipSize = 5;
        weaponManager.Weapon.ClipAmmoAmount = 0;

        weaponManager.ReloadWeapon();

        yield return new WaitForSeconds(4);

        Assert.AreEqual(expectedClipAmount, weaponManager.Weapon.ClipAmmoAmount);
        Assert.AreEqual(expectedAmmoAmount, weaponManager.Weapon.AmmoAmount);
    }

    [UnityTest]
    public IEnumerator WhenReloadingShouldLowerAmmoAmount()
    {
        int expectedAmmoAmount = 0;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        
        weaponManager.Weapon.MaxAmmo = 30;
        weaponManager.Weapon.AmmoAmount = 5;

        weaponManager.Weapon.MaxClipSize = 5;
        weaponManager.Weapon.ClipAmmoAmount = 0;

        weaponManager.ReloadWeapon();

        yield return new WaitForSeconds(4);

        Assert.AreEqual(expectedAmmoAmount, weaponManager.Weapon.AmmoAmount);
    }
}
