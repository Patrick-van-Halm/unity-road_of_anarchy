using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    [SerializeField] private GameObject _eliminatedPrefab;

    private void Awake()
    {
        if (_healthBarPrefab is not null)
        {
            GameObject healthBarInstance = Instantiate(_healthBarPrefab);
            _healthBar = healthBarInstance.GetComponent<HealthBar>();
        }
    }

    public void OnHealthChanged(float currentHealth)
    {
        _healthBar.SetHealthText(currentHealth);
        ShowEliminatedUIWhenDead(currentHealth);
    }

    private void ShowEliminatedUIWhenDead(float health)
    {
        if (health > 0) return;
        Instantiate(_eliminatedPrefab);
    }
}
