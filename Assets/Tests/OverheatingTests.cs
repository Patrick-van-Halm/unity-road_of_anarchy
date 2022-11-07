using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OverheatingTests : MonoBehaviour
{
    private GameObject GameObject {get; set;} 

    [SetUp]
    public void InitTests()
    {
        GameObject = new GameObject(nameof(OverheatingTests));
        GameObject.AddComponent<PlayerHUDComponent>();
        Gunner player = GameObject.AddComponent<Gunner>();
        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        weaponManager.Weapon = ScriptableObject.CreateInstance<Weapon>();
    }

#region Non-public function tests
    [Test]
    public void WhenCallingSubtractHeatValueShouldLowerCurrentHeatValue()
    {
        float expectedHeatValue = 4f;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        ConfigureWeaponHeatValues(weaponManager.Weapon);

        // Stop the heat counter from lowering for this test
        weaponManager.Weapon.HeatReducingValue = 0f;
        weaponManager.Weapon.CurrentHeatValue = 4f;

        weaponManager.GetType().GetMethod("SubtractHeatValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(weaponManager);

        Assert.AreEqual(expectedHeatValue, weaponManager.Weapon.CurrentHeatValue);
    }

    [UnityTest]
    public IEnumerator WhenShootingHeatValueShouldDecrease()
    {
        float expectedHeatValue = 0f;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        ConfigureWeaponAmmoValues(weaponManager.Weapon);
        ConfigureWeaponHeatValues(weaponManager.Weapon);

        // Add a delay of 1 second before subtracting the value
        weaponManager.Weapon.CooldownTime = 1f;

        // Fire the weapon once
        weaponManager.TryFireWeapon();

        // Wait a little longer to give time to cooldown
        yield return new WaitForSeconds(2);

        Assert.AreEqual(expectedHeatValue, weaponManager.Weapon.CurrentHeatValue);
    }

    [UnityTest]
    public IEnumerator WhenOverheatedShouldResetHeatCounterAfterCooldown()
    {
        float expectedHeatValue = 0f;

        WeaponManager weaponManager = GameObject.GetComponent<WeaponManager>();
        
        // Make sure the weapon can fire once
        weaponManager.Weapon.ClipAmmoAmount = 1;
        // Add a delay of 1 second before subtracting the value
        weaponManager.Weapon.CooldownTime = 1f;

        weaponManager.Weapon.HeatMaxValue = 4f;

        // Dont allow system to automaticly reduce the value for simulation
        weaponManager.Weapon.HeatReducingValue = 0f;
        // Make sure the first shot overheats the weapon
        weaponManager.Weapon.HeatPerShotValue = 4f;

        weaponManager.Weapon.CurrentHeatValue = 0f;

        // Check if overheated
        weaponManager.GetType().GetMethod("CheckGunOverheat", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(weaponManager);

        // Wait a little longer to give time to cooldown
        yield return new WaitForSeconds(2);

        Assert.AreEqual(expectedHeatValue, weaponManager.Weapon.CurrentHeatValue);
    }
#endregion

    private void ConfigureWeaponHeatValues(Weapon weapon)
    {
        weapon.HeatMaxValue = 40f;
        weapon.HeatReducingValue = 4f;
        weapon.HeatPerShotValue = 4f;
        weapon.CurrentHeatValue = 0f;
    }

    private void ConfigureWeaponAmmoValues(Weapon weapon)
    {
        weapon.ClipAmmoAmount = 1;
        weapon.AmmoAmount = 1;
        weapon.MaxAmmo = 10;
    }
}
