using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    [SerializeField] private GameObject _hitMarker;
    [SerializeField] private float _hitMarkerActiveTime = 0.2f;
    [SerializeField] private GameObject _eliminatedPrefab;

    private void Awake()
    {
        if (_healthBarPrefab is not null)
        {
            GameObject healthBarInstance = Instantiate(_healthBarPrefab);
            _healthBar = healthBarInstance.GetComponent<HealthBar>();
        }

        if (_hitMarker is not null)
        {
            _hitMarker = GameObject.Instantiate(_hitMarker);
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

    public void OnEnemyHit()
    {
        if (_hitMarker is null)
            return;

        _hitMarker.SetActive(true);
        StartCoroutine(nameof(CoroHitmarkerActiveTime), _hitMarkerActiveTime);
    }

    private IEnumerator CoroHitmarkerActiveTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        _hitMarker.SetActive(false);
    }
}