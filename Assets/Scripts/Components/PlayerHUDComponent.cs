using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDComponent : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _playerHudParent;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private GameObject _heatBarPrefab;
    private HealthBar _healthBar;
    private HeatBar _heatBar;
    [SerializeField] private GameObject _playerNamesPrefab;
    [SerializeField] private KillFeedUI _killFeed;

    [SerializeField] private GameObject _hitMarker;
    [SerializeField] private float _hitMarkerActiveTime = 0.2f;
    [SerializeField] private GameObject _eliminatedPrefab;
    [SerializeField] private GameObject _winPrefab;
    [SerializeField] private GameObject _ammoUI;
    [SerializeField] private GameObject _crosshairUI;
    [SerializeField] private RawImage _minimapTexture;
    [SerializeField] private Vector3 _minimapCamOffset;
    [SerializeField] private LayerMask _minimapLayers;
    [SerializeField] private PlayerNameUI _playerNameUI;
    [SerializeField] private Vector3 _playerNameUIOffset;

    [SerializeField] private GameSettings _gameSettings;

    private void Awake()
    {
        if (_healthBarPrefab is not null)
        {
            GameObject healthBarInstance = Instantiate(_healthBarPrefab, _playerHudParent);
            _healthBar = healthBarInstance.GetComponent<HealthBar>();
        }

        if (_hitMarker is not null)
        {
            _hitMarker = GameObject.Instantiate(_hitMarker);
        }

        if (_healthBarPrefab is not null)
        {
            _heatBar = Instantiate(_heatBarPrefab, _playerHudParent).GetComponent<HeatBar>();
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
        winUI.GetComponent<YouWinUI>().ShowWinUI(text);
    }

    public void ShowWinUI(int position, string text)
    {
        GameObject winUI = Instantiate(_winPrefab, _canvas);
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

    internal void SetPlayerNames(Player driver, Player gunner)
    {
        string driverName = driver.name;
        string gunnerName = gunner.name;      

        if (!driver.isLocalPlayer && _gameSettings.HideOtherUsernames) driverName = "Driver";
        else driver.OnNameChanged.AddListener(_playerNameUI.SetDriverName);

        if (!gunner.isLocalPlayer && _gameSettings.HideOtherUsernames) gunnerName = "Gunner";
        else gunner.OnNameChanged.AddListener(_playerNameUI.SetGunnerName);

        _playerNameUI.SetDriverName(driverName);
        _playerNameUI.SetGunnerName(gunnerName);
    }

    public void CreateTeamPlayerNamesUI(Team team)
    {
        GameObject instance = Instantiate(_playerNamesPrefab);

        Player driver = team.DriverPlayer;
        Player gunner = team.GunnerPlayer;
        WorldSpacePlayerNameUI worldSpacePlayerNameUI = instance.GetComponent<WorldSpacePlayerNameUI>();

        string driverName = driver.name;
        string gunnerName = gunner.name;

        if (NetworkClient.connection.identity.GetComponent<Player>() != driver && _gameSettings.HideOtherUsernames) driverName = "Driver";
        else driver.OnNameChanged.AddListener(worldSpacePlayerNameUI.SetDriverName);

        if (NetworkClient.connection.identity.GetComponent<Player>() != gunner && _gameSettings.HideOtherUsernames) gunnerName = "Gunner";
        else gunner.OnNameChanged.AddListener(worldSpacePlayerNameUI.SetGunnerName);

        worldSpacePlayerNameUI.SetDriverName(driverName);
        worldSpacePlayerNameUI.SetGunnerName(gunnerName);

        GlueToPosition glue = instance.AddComponent<GlueToPosition>();
        glue.Target = driver.gameObject.transform;
        glue.LocalPosition = _playerNameUIOffset;

        instance.GetComponent<RotateToPlayer>().CamTransform = NetworkClient.connection.identity.GetComponent<Player>().PlayerCam.transform;
    }

    public void CreateMinimap(GameObject car)
    {
        RenderTexture texture = new RenderTexture(512, 512, 16);
        texture.Create();

        GameObject minimapCamera = new GameObject("Minimap Camera");
        Camera minimapCam = minimapCamera.AddComponent<Camera>();
        minimapCam.transform.rotation = Quaternion.Euler(90, 0, 0);
        minimapCam.transform.position = car.transform.position + _minimapCamOffset;

        minimapCam.targetTexture = texture;
        minimapCam.cullingMask = _minimapLayers;
        minimapCam.Render();

        _minimapTexture.texture = texture;

        MinimapBehaviour minimap = minimapCamera.AddComponent<MinimapBehaviour>();
        minimap.LockOn = car.transform;
        minimap.offset = _minimapCamOffset;
    }
}