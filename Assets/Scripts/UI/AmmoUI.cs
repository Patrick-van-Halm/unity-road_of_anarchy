using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private GameObject _ammoAmountTextObject;
    [SerializeField] private GameObject _clipAmmoAmountTextObject;

    private WeaponManager _weaponManager;
    private TMP_Text _ammoAmountText;
    private TMP_Text _clipAmmoAmountText;

    void Start()
    {
        _ammoAmountText = _ammoAmountTextObject.GetComponent<TMP_Text>();
        _clipAmmoAmountText = _clipAmmoAmountTextObject.GetComponent<TMP_Text>();
    }

    public void UpdateUI(int clipAmmoAmount, int ammoAmount)
    {
        _clipAmmoAmountText.text = clipAmmoAmount.ToString();
        _ammoAmountText.text = ammoAmount.ToString();
    }
}
