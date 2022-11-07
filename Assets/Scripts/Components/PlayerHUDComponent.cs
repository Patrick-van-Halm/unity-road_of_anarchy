using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private GameObject _heatBarPrefab;
    private HealthBar _healthBar;
    private HeatBar _heatBar;
    [SerializeField] private KillFeedUI _killFeed;

    [SerializeField] private GameObject _hitMarker;
    [SerializeField] private float _hitMarkerActiveTime = 0.2f;
    [SerializeField] private GameObject _eliminatedPrefab;
    [SerializeField] private GameObject _winPrefab;
    [SerializeField] private GameObject _ammoUI;
    [SerializeField] private GameObject _crosshairUI;

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

        if (_healthBarPrefab is not null)
        {
            _heatBar = Instantiate(_heatBarPrefab).GetComponent<HeatBar>();
        }
    }

    public void OnHealthChanged(float currentHealth)
    {
        _healthBar.SetHealthText(currentHealth);
    }

    public void OnHeatChanged(float currentHeat)
    {
        _heatBar.UpdateHeatText(currentHeat);
    }

    public void OnKillFeedMessage(string msg)
    {
        _killFeed.ShowMessage(msg);
    }

    public void ShowEliminatedUI()
    {
        Instantiate(_eliminatedPrefab, _canvas);
    }

    public void ShowWinUI(string text)
    {
        GameObject winUI = Instantiate(_winPrefab, _canvas);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        winUI.GetComponent<YouWinUI>().ShowWinUI(text);
    }

    public void ShowWinUI(int position, string text)
    {
        GameObject winUI = Instantiate(_winPrefab, _canvas);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        winUI.GetComponent<YouWinUI>().ShowWinUI(position, text);

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

    public void HideGunnerUI()
    {
        _ammoUI.SetActive(false);
        _crosshairUI.SetActive(false);
        _heatBar.gameObject.SetActive(false);
    }

    public void SetMaxHeat(float heatMaxValue)
    {
        _heatBar.SetMaxHeat(heatMaxValue);
    }
}