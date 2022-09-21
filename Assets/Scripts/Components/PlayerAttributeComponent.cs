using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerAttributeComponent : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCurrentHealthChanged))] public float CurrentHealth;
    public float MaxHealth = 100f;
    public UnityEvent<float> OnHealthChanged;

    // Set variables in start and only for server
    private void Start()
    {
        if (!isServer) return;
        CurrentHealth = MaxHealth;
    }

    // Calls the apply damage on the server
    [Command(requiresAuthority = false)]
    public void CmdApplyDamage(float amount)
    {
        ApplyDamage(amount);
    }

    // Subtracts a value from the CurrentHealth property as long as the damage is in range.
    private void ApplyDamage(float amount)
    {
        if (CurrentHealth < 1)
            return;

        float damage = Mathf.Clamp(amount, 0f, MaxHealth);

        CurrentHealth -= damage;
    }

    // Hook for when server changed current health
    private void OnCurrentHealthChanged(float oldValue, float newValue)
    {
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}
