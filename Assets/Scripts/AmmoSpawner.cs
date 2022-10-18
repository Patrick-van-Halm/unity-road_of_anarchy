using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] GameObject _ammoBox;
    [SerializeField] Weapon _weapon;

    private int _ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            AddAmmo();
            StartCoroutine(RespawnAmmo());
        }
    }

    /// <summary>
    /// Adds ammo and destroys ammo pickup
    /// </summary>
    private void AddAmmo()
    {
        _ammoBox.SetActive(false);

        // Code for adding ammo
        _weapon.AmmoAmount += _ammoAmount;
        if (_weapon.AmmoAmount > _weapon.MaxAmmo) _weapon.AmmoAmount = _weapon.MaxAmmo;
    }

    /// <summary>
    /// Respawns ammo after amount of seconds
    /// </summary>
    IEnumerator RespawnAmmo()
    {
        yield return new WaitForSeconds(3f);

        _ammoBox.SetActive(true);
    }
}
