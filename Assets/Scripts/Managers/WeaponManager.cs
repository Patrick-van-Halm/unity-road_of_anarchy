using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
using System;

public class WeaponManager : NetworkBehaviour
{
    #region Private fields
    private float _nextShot;
    private Gunner _gunner;
    private bool isInWater;
    #endregion

    #region Bullet
    [Header("Bullet")]
    [SerializeField] private GameObject _bullet;
    #endregion

    #region Weapon
    [Header("Weapon")]
    public Weapon Weapon;
    [SerializeField] private Transform _weaponMuzzlePosition;
    [SerializeField] private Transform _weaponObject;

    #endregion

    #region Gunner
    [SerializeField] private Camera _gunnerCamera;
    [SerializeField] private LayerMask _hittableLayers;
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent OnEnemyHit = new UnityEvent();
    public UnityEvent<float> OnHeatChanged = new UnityEvent<float>();
    #endregion

    #region Unity Lifecycle methods
    private void Awake()
    {
        _gunner = GetComponent<Gunner>();
    }

    private void Start()
    {
        Weapon.WeaponState = WeaponState.ReadyToShoot;

        OnEnemyHit.AddListener(FindObjectOfType<PlayerHUDComponent>().OnEnemyHit);
        InvokeRepeating(nameof(SubtractHeatValue), Weapon.CooldownTime, Weapon.CooldownTime);
        Weapon.AmmoAmount = 0;
        Weapon.ClipAmmoAmount = 0;
    }
    #endregion

    #region Shooting
    public void TryFireWeapon()
    {
        if (Weapon.WeaponState == WeaponState.ReadyToShoot && 
        HasEnoughAmmoInClip() && HasWaitedForShotDelay())
        {
            Weapon.WeaponState = WeaponState.Firing;
            PlaySoundEffectFMOD(Weapon.FireSoundRef, Vector3.zero);
            if(_gunnerCamera) PerformRaycast();
            

            // Calculate the time to wait before allowing the next shot
            _nextShot = Time.time + Weapon.FireRateDelay;

            // Add to heatvalue
            Weapon.CurrentHeatValue += Weapon.HeatPerShotValue;
            OnHeatChanged?.Invoke(Weapon.CurrentHeatValue);

            // Check if the current shot has overheated the gun. If it does, then start cooldown
            CheckGunOverheat();

            if (Weapon.WeaponState != WeaponState.Overheated) Weapon.WeaponState = WeaponState.ReadyToShoot;
            //CmdSetBulletDirection(bulletDirection);
        }
    }

    private void PerformRaycast()
    {
        // Raycast in the middle of the camera
        Ray ray = _gunnerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Stores the location of the point where the target is hit
        Vector3 targetPoint = ray.GetPoint(Weapon.WeaponRange);

        if (Physics.Raycast(ray, out RaycastHit hit, Weapon.WeaponRange, _hittableLayers))
        {
            // If the target hit is the enemy, get the Interface and call the ApplyDamage function
            if (IsHittable(hit) is IDamageable target)
            {
                Debug.Log("Target is enemy");
                PlaySoundEffectFMOD(Weapon.HitSoundRef, hit.point);

                target.CmdApplyDamage(Weapon.DamageAmount);
                OnEnemyHit.Invoke();
            }
        }

        // Calculate the direction in which the weapon needs to fire
        Weapon.ClipAmmoAmount--;
        CmdInstatiateBullet(CalculateFireDirection(targetPoint));
    }

    private void CheckGunOverheat()
    {
        if (HasExceededOverheatLimit())
        {
            PlaySoundEffectFMOD(Weapon.OverheatSoundRef, Vector3.zero);
            StartCoroutine(nameof(CoroWaitForCooldown), Weapon.OverheatedTime);
        }
    }

    private IDamageable IsHittable(RaycastHit hit)
    {
        if (hit.collider.GetComponent<IDamageable>() is IDamageable target)
            return target;

        return null;
    }

    private Vector3 CalculateFireDirection(Vector3 targetPoint)
    {
        return targetPoint - _weaponMuzzlePosition.transform.position;
    }
    #endregion

    #region Networking

    [Command(requiresAuthority = true)]
    private void CmdInstatiateBullet(Vector3 bulletDirection)
    {
        RpcInstantiateBullet(bulletDirection);
    }

    [ClientRpc]
    private void RpcInstantiateBullet(Vector3 bulletDirection)
    {
        InstantiateBullet(bulletDirection);
    }

    #endregion

    #region Bullets and Projectiles
    private void InstantiateBullet(Vector3 bulletDirection)
    {
        // Instantiate bullet
        GameObject currentBullet = Instantiate(_bullet, _weaponMuzzlePosition.transform.position, Quaternion.identity);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = bulletDirection.normalized;
    }
    #endregion

    #region Sound
    private void PlaySoundEffectFMOD(EventReference soundReference, Vector3 location)
    {
        if (soundReference.IsNull) return;
        EventInstance soundInstance = RuntimeManager.CreateInstance(soundReference);

        // No location was passed in
        if (location == Vector3.zero)
        {
            soundInstance.start();
            return;
        }
        else
        {
            // When a Vector3 is provided, play sound at location
            soundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(location));
            soundInstance.start();
        }
    }

    #endregion

    #region Weapon Handling
    private bool HasEnoughAmmoInClip()
    {
        return Weapon.ClipAmmoAmount > 0;
    }

    private void SubtractHeatValue()
    {
        // Dont reduce heat when weapon is firing
        // If the weapon is overheated, dont reduce value (CoroWaitForCooldown will reset)
        if (Weapon.WeaponState == WeaponState.Overheated || Weapon.WeaponState == WeaponState.Firing || Weapon.CurrentHeatValue == 0f)
            return;
    
        Weapon.CurrentHeatValue -= isInWater ? Weapon.HeatReducingValueInWater : Weapon.HeatReducingValue;
        if(Weapon.CurrentHeatValue < 0) Weapon.CurrentHeatValue = 0;
        OnHeatChanged?.Invoke(Weapon.CurrentHeatValue);
    }

    private bool HasExceededOverheatLimit()
    {
        return Weapon.CurrentHeatValue >= Weapon.HeatMaxValue;
    }

    private bool HasWaitedForShotDelay()
    {
        return Time.time > _nextShot;
    }

    private bool HasEnoughAmmoToReload()
    {
        // Change so player can always reload
        //return Weapon.MaxClipSize <= Weapon.AmmoAmount;
        return Weapon.AmmoAmount > 0;
    }

    private bool HasFullClip()
    {
        return Weapon.ClipAmmoAmount == Weapon.MaxClipSize;
    }

    public void ReloadWeapon()
    {
        // Only allow reload if player has enough ammo, and not already reloading
        if (HasEnoughAmmoToReload() && Weapon.WeaponState != WeaponState.Reloading)
            StartCoroutine(nameof(CoroReloadDelay), Weapon.ReloadTime);
    }

    private IEnumerator CoroReloadDelay(float delayTime)
    {
        Weapon.WeaponState = WeaponState.Reloading;

        yield return new WaitForSeconds(delayTime);

        // Calculate how much ammo is required to fully reload
        int ammoRequiredFullReload = Weapon.MaxClipSize - Weapon.ClipAmmoAmount;

        // Reload depending on how much ammo is in storage
        if (Weapon.AmmoAmount < ammoRequiredFullReload)
        {
            Weapon.ClipAmmoAmount += Weapon.AmmoAmount;
            Weapon.AmmoAmount = 0;
        }
        else
        {
            Weapon.ClipAmmoAmount = Weapon.MaxClipSize;
            Weapon.AmmoAmount -= ammoRequiredFullReload;
        }

        // Refill the clip size and subtract a clip from the total ammo amount
        //Weapon.ClipAmmoAmount = Weapon.MaxClipSize;
        //Weapon.AmmoAmount -= Weapon.MaxClipSize;

        Weapon.WeaponState = WeaponState.ReadyToShoot;
    }

    private IEnumerator CoroWaitForCooldown(float delayTime)
    {
        Weapon.WeaponState = WeaponState.Overheated;

        yield return new WaitForSeconds(delayTime);

        Weapon.CurrentHeatValue = 0f;
        OnHeatChanged?.Invoke(Weapon.CurrentHeatValue);

        Weapon.WeaponState = WeaponState.ReadyToShoot;
    }

    public void WaterCooldown(bool inWater)
    {
        isInWater = inWater;
    }
    #endregion
}