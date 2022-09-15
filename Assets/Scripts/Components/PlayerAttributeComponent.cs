using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerAttributeComponent : MonoBehaviour
{
    public float MaxHealth {get; set;}
    public float CurrentHealth {get; set;}
    public UnityEvent<float> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = 100f;
        MaxHealth = 100f;
    }

    // Subtracts a value from the CurrentHealth property as long as the damage is in range.
    public void ApplyDamage(float amount)
    {
        if (CurrentHealth < 1)
            return;

        float damage = Mathf.Clamp(amount, 0f, MaxHealth);

        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}
