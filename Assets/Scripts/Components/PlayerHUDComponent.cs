using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;
    [SerializeField] private KillFeedUI _killFeed;

    [SerializeField] private GameObject _hitMarker;
    [SerializeField] private float _hitMarkerActiveTime = 0.2f;
    [SerializeField] private GameObject _eliminatedPrefab;
    [SerializeField] private GameObject _winPrefab;

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
}