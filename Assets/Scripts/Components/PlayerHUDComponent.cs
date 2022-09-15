using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    private void Awake()
    {
        if (_healthBarPrefab is not null)
        {
            GameObject healthBarInstance = GameObject.Instantiate(_healthBarPrefab);
            _healthBar = healthBarInstance.GetComponent<HealthBar>();
        }
    }

    public void OnHealthChanged(float currentHealth)
    {
        _healthBar?.SetHealthText(currentHealth);
    }
}
