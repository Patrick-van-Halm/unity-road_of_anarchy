using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponManager))]
public class Gunner : Player
{
    private KeyboardInput _input;
    private WeaponManager _weaponManager;
    private AmmoUI _ammoUI;

    private void Awake()
    {
        _input = GetComponent<KeyboardInput>();
        _weaponManager = GetComponent<WeaponManager>();
        _ammoUI = FindObjectOfType<AmmoUI>();
    }

    private void Update()
    {
        if (!isLocalPlayer || _input.SettingsOpen) return;
        if (_input.Shooting) _weaponManager.TryFireWeapon();
        if (_input.Reload) _weaponManager.ReloadWeapon();
        if (_gameSettings.AutomaticReload && _weaponManager.Weapon.ClipAmmoAmount == 0) _weaponManager.ReloadWeapon();
        _ammoUI.UpdateUI(_weaponManager.Weapon.ClipAmmoAmount, _weaponManager.Weapon.AmmoAmount);
    }
}
