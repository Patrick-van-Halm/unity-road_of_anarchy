using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(AttributeComponent))]
public class Vehicle : Player
{
    protected AttributeComponent _attributes;

    private void Awake()
    {
        _attributes = GetComponent<AttributeComponent>();
    }
}
