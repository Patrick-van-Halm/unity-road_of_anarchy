using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Powerups/GuidedMissile")]
public class GuidedMissile : BasePowerup
{
    [Header("VFX")]
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _missileFireEffect;
    [SerializeField] private GameObject _missileExplosionEffect;

    [Header("Missile Settings")]
    [SerializeField] private float _missileSpeed;
    [SerializeField] private float _missileRotationSpeed;
    [SerializeField] private float _missileDamage;


    public GameObject MissilePrefab { get { return _missilePrefab; } }
    public GameObject MissileFireEffect { get { return _missileFireEffect;  } }
    public GameObject MissileExplosionEffect { get { return _missileExplosionEffect; } }
    public float MissileSpeed { get { return _missileSpeed; } }
    public float MissileRotationSpeed { get { return _missileRotationSpeed; } }
    public float MissileDamage { get { return _missileDamage; } }


    public override void Pickup()
    {
        NetworkClient.connection.identity.GetComponent<ActivateGuidedMissile>().ActivateMissile();
    }
}
