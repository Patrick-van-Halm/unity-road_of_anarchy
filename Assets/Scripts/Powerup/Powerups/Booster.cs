using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Powerups/Booster")]
public class Booster : BasePowerup
{
    [SerializeField] private float _speedBoost;
    [SerializeField] private float _additiveMaxSpeed;
    [SerializeField] private float _duration;

    public override void Pickup()
    {
        NetworkClient.connection.identity.GetComponentInChildren<NewKartScript>().AddPowerup(new()
        {
            MaxTime = _duration,
            modifiers = new()
            {
                Acceleration = _speedBoost,
                TopSpeed = _additiveMaxSpeed
            }
        }) ;
    }
}
