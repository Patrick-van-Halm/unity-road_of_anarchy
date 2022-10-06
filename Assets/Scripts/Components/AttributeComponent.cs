using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttributeComponent : NetworkBehaviour, IDamageable
{
    [SyncVar(hook = nameof(OnCurrentHealthChanged))] public float CurrentHealth;
    public float MaxHealth = 100f;

    public UnityEvent<float> OnHealthChanged;
    
    public Team LastDamagedBy => _lastDamagedBy;
    [SyncVar] private Team _lastDamagedBy;

    // Set variables in start and only for server
    private void Start()
    {
        //if (!isServer) return;
        CurrentHealth = MaxHealth;
    }

    // Calls the apply damage on the server
    [Command(requiresAuthority = false)]
    public void CmdApplyDamage(float amount, NetworkConnectionToClient connectionToClient = null)
    {
        ApplyDamage(amount);
        _lastDamagedBy = connectionToClient.identity.GetComponent<Player>().Team;
    }

    // Subtracts a value from the CurrentHealth property as long as the damage is in range.
    public void ApplyDamage(float amount)
    {
        if (CurrentHealth < 1)
            return;

        float damage = Mathf.Clamp(amount, 0f, MaxHealth);

        CurrentHealth -= damage;

        if (!NetworkClient.active) OnHealthChanged?.Invoke(CurrentHealth);
    }

    // Hook for when server changed current health
    private void OnCurrentHealthChanged(float oldValue, float newValue)
    {
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}
