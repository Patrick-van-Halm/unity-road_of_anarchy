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
    }

    protected override void Explode()
    {
        _inputEnabled = false;
        Debug.Log("Player exploded, input is disabled. The object still exists.");
        //Destroy(gameObject);
    }
}
