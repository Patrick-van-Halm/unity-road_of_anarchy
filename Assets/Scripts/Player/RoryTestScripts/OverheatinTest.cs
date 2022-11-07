using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheatinTest : MonoBehaviour
{
    public WeaponManager WeaponManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            WeaponManager.TryFireWeapon();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            WeaponManager.ReloadWeapon();
        }
    }
}
