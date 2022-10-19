using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class Gunner : Player
{
    private KeyboardInput _input;
    private WeaponManager _weaponManager;
    [SerializeField] GameSettings _gameSettings;

    private void Awake()
    {
        _input = GetComponent<KeyboardInput>();
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (_input.Shooting) _weaponManager.TryFireWeapon();
        if (_input.Reload) _weaponManager.ReloadWeapon();
        if (_gameSettings.AutomaticReload && _weaponManager.Weapon.ClipAmmoAmount == 0) _weaponManager.ReloadWeapon();           
    }
}
