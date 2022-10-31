using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;

public class WeaponManager : NetworkBehaviour
{
    #region Private fields
    private AudioListener _listener;
    private bool _canShoot;
    private bool _isReloading;
    private float _nextShot;
    private Gunner _gunner;
    #endregion

    #region Bullet
    [Header("Bullets")]
    [SerializeField] private GameObject _bullet;
    #endregion

    #region Weapon
    [Header("Weapon")]
    public Weapon Weapon;
    [SerializeField] private Transform _weaponMuzzlePosition;
    [SerializeField] private Transform _weaponObject;

    #endregion

    #region Gunner
    [SerializeField] private string _targetTagName = "Vehicle";
    [SerializeField] private Camera _gunnerCamera;
    #endregion

    private void Awake()
    {
        _canShoot = true;
        _isReloading = false;
        _listener = FindObjectOfType<AudioListener>();
        _gunner = GetComponent<Gunner>();
    }

    private void Start()
    {
        OnEnemyHit.AddListener(FindObjectOfType<PlayerHUDComponent>().OnEnemyHit);
    }

    #region Shooting
    public UnityEvent OnEnemyHit = new UnityEvent();


    public void TryFireWeapon()
    {
        // If the weapon is allowed to shoot, and has enough ammo try shooting
        if (_canShoot && HasEnoughAmmoInClip() && HasWaitedForShotDelay())
        {
            // Play FMOD fire effect
            //PlaySoundEffect(Weapon.WeaponFireSound);
            PlaySoundEffectFMOD(Weapon._fireSoundRef, Vector3.zero);

            Weapon.ClipAmmoAmount--;

            // Calculate the time to wait before allowing the next shot. Depends on _currentWeapon.FireRateDelayInSeconds
            _nextShot = CalculateNextShotDelay();

            // Raycast in the middle of the camera
            Ray ray = _gunnerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Shoot
            PerformRaycast(ray);
        }
    }

    private void PerformRaycast(Ray ray)
    {
        // Stores the location of the point where the target is hit
        Vector3 targetPoint = ray.GetPoint(Weapon.WeaponRange);

        if (Physics.Raycast(ray, out RaycastHit hit, Weapon.WeaponRange))
        {
            // If the target hit is the enemy, get the Interface and call the ApplyDamage function
            if (IsHittable(hit) is IDamageable target)
            {
                Debug.Log("Target is enemy");

                // Store the hit location as Vector3
                targetPoint = hit.point;

                target.CmdApplyDamage(Weapon.DamageAmount);

                //PlaySoundEffect(Weapon.HitSound);
                PlaySoundEffectFMOD(Weapon._hitSoundRef, hit.point);

                OnEnemyHit.Invoke();
            }
        }

        // Calculate the direction in which the weapon needs to fire
        InstantiateBullet(CalculateFireDirection(targetPoint));

        //CmdSetBulletDirection(bulletDirection);
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

    #region Bullets
    private void InstantiateBullet(Vector3 bulletDirection)
    {
        // Instantiate bullet
        GameObject currentBullet = Instantiate(_bullet, _weaponMuzzlePosition.transform.position, Quaternion.identity);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = bulletDirection.normalized;   
    }
    #endregion

    #region Sound
    public void PlaySoundEffectFMOD(EventReference soundReference, Vector3 location)
    {
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
    private float CalculateNextShotDelay()
    {
        return Time.time + Weapon.FireRateDelayInSeconds;
    }

    private bool HasEnoughAmmoInClip()
    {
        return Weapon.ClipAmmoAmount > 0;
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
        if (HasEnoughAmmoToReload() && !HasFullClip() && !_isReloading)
            StartCoroutine(nameof(CoroReloadDelay), Weapon.ReloadTimeInSeconds);
    }

    private IEnumerator CoroReloadDelay(float delayTime)
    {
        _canShoot = false;
        _isReloading = true;

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

        _isReloading = false;
        _canShoot = true;
    }
    #endregion
}