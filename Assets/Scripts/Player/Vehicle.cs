using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(AttributeComponent))]
public class Vehicle : NetworkBehaviour
{
    protected AttributeComponent _attributes;

    private void Awake()
    {
        _attributes = GetComponent<AttributeComponent>();
    }

    public virtual void OnHealthChanged(float value)
    {
        Debug.Log($"[{name}] has [{_attributes.CurrentHealth}] health left.");

        if (_attributes.CurrentHealth <= 0f)
            Explode();
    }

    protected virtual void Explode()
    {
        Debug.Log($"[{name}] has exploded.");

        Destroy(gameObject);
    }
}
