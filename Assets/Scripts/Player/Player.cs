using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    [SyncVar] public Team Team;
    public UnityEvent<string> OnNameChanged = new UnityEvent<string>();
    [SyncVar(hook = nameof(OnNameValueChanged))] public string name = "Johnson";
    [SerializeField] protected GameSettings _gameSettings;

    public Camera PlayerCam => playerCam;
    [SerializeField] private Camera playerCam;

    private PlayerHUDComponent _hudComponent;

    [SyncVar] public bool IsReady;

    private void Start()
    {
        if (isLocalPlayer) CmdReadyPlayer();      
        _hudComponent = FindObjectOfType<PlayerHUDComponent>();

        if (!isLocalPlayer) return;
        SetUsername(_gameSettings.HideOwnUsername ? "Player" : _gameSettings.Username);
    }

    [Command(requiresAuthority = true)]
    public void SetUsername(string name)
    {
        this.name = name;
    }

    public void OnNameValueChanged(string oldValue, string newValue)
    {
        OnNameChanged?.Invoke(newValue);
    }

    [Command]
    private void CmdReadyPlayer()
    {
        IsReady = true;
        CountdownManager.Instance?.CheckIfStartCountdown();
    }

    [TargetRpc]
    public void TargetOnTeamKill(NetworkConnection conn)
    {
        _hudComponent?.OnKillFeedMessage("Your team has killed another team!");
    }

    [TargetRpc]
    public void TargetOnEliminated(NetworkConnection conn)
    {
        _hudComponent?.ShowEliminatedUI();
    }
}

public class Team
{
    public Gunner Gunner => GunnerIdentity.GetComponent<Gunner>();
    public Player GunnerPlayer => GunnerIdentity.GetComponent<Player>();
    public Spectator GunnerSpectator => GunnerIdentity.GetComponent<Spectator>();
    public NetworkIdentity GunnerIdentity;
    public int GunnerClientId;

    public Vehicle Vehicle => DriverIdentity.GetComponent<Vehicle>();
    public Player DriverPlayer => DriverIdentity.GetComponent<Player>();

    public Spectator DriverSpectator => DriverIdentity.GetComponent<Spectator>();
    public NetworkIdentity DriverIdentity;
    public int DriverClientId;
}
