using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownEffectHandler : NetworkBehaviour
{
    private IEnumerator _coroToDeactivateSlowdownBullet;
    public bool SlowdownBulletsActive { get; private set; } = false;
    public SlowdownBullet SlowdownBulletProperties { get; private set; }

    private void Start()
    {
        NetworkClient.connection.identity.GetComponent<Player>().Team.GunnerPlayer.GetComponent<WeaponManager>().SetSlowdownEffectHandler(this);
    }

    public void ActivateSlowdownBullets(SlowdownBullet slowdownBullet)
    {
        if (!SlowdownBulletsActive)
            SlowdownBulletsActive = true;
        else
            StopCoroutine(_coroToDeactivateSlowdownBullet);

        SlowdownBulletProperties = slowdownBullet;
        _coroToDeactivateSlowdownBullet = CoroDeactivateSlowdownBullet(slowdownBullet.ActiveDuration);
        StartCoroutine(_coroToDeactivateSlowdownBullet);
    }

    private IEnumerator CoroDeactivateSlowdownBullet(float activeDuration)
    {
        yield return new WaitForSeconds(activeDuration);
        SlowdownBulletsActive = false;
    }

    [Command(requiresAuthority = false)]
    public void CmdApplySlowdownEffect(float effectDuration, float accelerationDecreaseAmount, float topSpeedDecreaseAmount, NetworkConnectionToClient networkConnectionToClient = null)
    {
        ApplySlowdownEffect(effectDuration, accelerationDecreaseAmount, topSpeedDecreaseAmount);
    }

    private void ApplySlowdownEffect(float effectDuration, float accelerationDecreaseAmount, float topSpeedDecreaseAmount)
    {
        NetworkClient.connection.identity.GetComponentInChildren<NewKartScript>().AddPowerup(new()
        {
            MaxTime = effectDuration,
            modifiers = new()
            {
                Acceleration = -accelerationDecreaseAmount,
                TopSpeed = -topSpeedDecreaseAmount
            }
        });
    }
}
