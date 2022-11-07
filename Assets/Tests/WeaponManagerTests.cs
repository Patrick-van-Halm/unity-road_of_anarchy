using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        GameObject.AddComponent<WeaponManager>();
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

    [Test]
    public void WhenNotEnoughAmmoInClipReturnsFalse()
    {
        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();

        // Make sure there is no ammo
        weaponManager.Weapon.ClipAmmoAmount = 0;
        weaponManager.Weapon.AmmoAmount = 0;
        weaponManager.Weapon.MaxAmmo = 10;

        bool result = (bool)weaponManager.GetType().GetMethod("HasEnoughAmmoInClip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(weaponManager);

        Assert.IsFalse(result);
    }

    [Test]
    public void WhenEnoughAmmoInClipReturnsTrue()
    {
        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();

        // Make sure there is enough ammo in clip
        ConfigureWeaponAmmoValues(weaponManager.Weapon);

        bool result = (bool)weaponManager.GetType().GetMethod("HasEnoughAmmoInClip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(weaponManager);

        Assert.IsTrue(result);
    }

    [Test]
    public void WhenReloadingWeaponStateShouldBeReloading()
    {
        WeaponState expectedWeaponState = WeaponState.Reloading;
        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        ConfigureWeaponAmmoValues(weaponManager.Weapon);

        weaponManager.ReloadWeapon();

        Assert.AreEqual(expectedWeaponState, weaponManager.Weapon.WeaponState);
    }

    private void ConfigureWeaponAmmoValues(Weapon weapon)
    {
        weapon.ClipAmmoAmount = 1;
        weapon.AmmoAmount = 1;
        weapon.MaxAmmo = 10;
    }
}
