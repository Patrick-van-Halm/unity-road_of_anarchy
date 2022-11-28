using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbyPlayer : NetworkBehaviour
{
    public enum RolesPreference 
    {
        Both,
        OnlyDriver,
        OnlyGunner
    }

    [SyncVar(hook = nameof(OnNameChanged))] [HideInInspector] public string Name;
    [SyncVar(hook = nameof(OnIsReadyChanged))] public bool IsReady;
    [SyncVar] public RolesPreference RolePreference;

    [SerializeField] private GameSettings _gameSettings;

    public UnityEvent<string> OnPlayerNameChanged;
    public UnityEvent<bool> OnReadyStateChanged;

    private void Start()
    {
        if (!isLocalPlayer) return;
       
        SetUsername(_gameSettings.HideOwnUsername ? "Player" : _gameSettings.Username);
    }

    private void OnNameChanged(string oldUsername, string newUsername)
    {
        // Invoke name change event
        OnPlayerNameChanged.Invoke(newUsername);
    }

    [Command(requiresAuthority = true)]
    public void SetUsername(string name)
    {
        Name = name;
    }

    private void OnIsReadyChanged(bool _, bool newValue)
    {
        // Invoke ready state change event
        OnReadyStateChanged.Invoke(newValue);
    } 

    [Command(requiresAuthority = true)]
    public void SetReady(bool ready)
    {
        IsReady = ready;
    }

    [Command]
    public void SetRolePreference(RolesPreference role)
    {
        RolePreference = role;
    }
}
