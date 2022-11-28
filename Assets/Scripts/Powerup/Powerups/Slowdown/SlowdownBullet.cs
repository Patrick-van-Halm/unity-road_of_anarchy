using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowdownBullet", menuName = "Scriptable Objects/Powerup/Slowdown Bullet")]
public class SlowdownBullet : BasePowerup
{
    public float ActiveDuration { get { return _activeDuration; } }
    [SerializeField] private float _activeDuration;

    public float EffectDuration { get { return _effectDuration; } }
    [SerializeField] private float _effectDuration;

    public float AccelerationDecreaseAmount { get { return _accelerationDecreaseAmount; } }
    [SerializeField] private float _accelerationDecreaseAmount;

    public float TopSpeedDecreaseAmount { get { return _topSpeedDecreaseAmount; } }
    [SerializeField] private float _topSpeedDecreaseAmount;

    public override void Pickup()
    {
        NetworkClient.connection.identity.GetComponent<SlowdownEffectHandler>().ActivateSlowdownBullets(this);
    }
}
