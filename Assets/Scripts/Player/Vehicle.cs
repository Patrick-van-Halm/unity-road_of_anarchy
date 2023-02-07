using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttributeComponent))]
public class Vehicle : Player
{
    protected AttributeComponent _attributes;
    
    [SyncVar(hook = nameof(InWaterValueChanged))] private bool _inWater;
    public UnityEvent<bool> OnInWater = new UnityEvent<bool>();

    private void Awake()
    {
        _attributes = GetComponent<AttributeComponent>();
    }

    private void InWaterValueChanged(bool prevVal, bool newVal)
    {
        OnInWater?.Invoke(newVal);
    }

    [Command(requiresAuthority = true)]
    public void InWater(bool inWater)
    {
        _inWater = inWater;
    }
}
