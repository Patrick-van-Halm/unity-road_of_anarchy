using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHUDComponent))]
[RequireComponent(typeof(WeaponManager))]
public class Player : Vehicle
{
    private bool _inputEnabled;
    [SerializeField] private PlayerHUDComponent _hud;
    [SerializeField] private WeaponManager _weaponManager;

    private void Awake()
    {
        _inputEnabled = true;
    }

    private void Update()
    {
        if (_inputEnabled)
        {
            if (Input.GetMouseButton(0))
                _weaponManager.TryFireWeapon();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Trying to apply damage to player");
                _attributes.ApplyDamage(10f);
            }

            if (Input.GetKeyDown(KeyCode.R))
                _weaponManager.ReloadWeapon();
        }
    }

    protected override void Explode()
    {
        _inputEnabled = false;
        Debug.Log("Player exploded, input is disabled. The object still exists.");
        //Destroy(gameObject);
    }
}
