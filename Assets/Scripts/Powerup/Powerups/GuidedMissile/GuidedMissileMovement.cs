using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileMovement : NetworkBehaviour
{
    [SerializeField] private GuidedMissile _guidedMissileScriptableObject;
    [SerializeField] private LayerMask _layerMask;

    private Rigidbody _missileRigidBody;
    [SyncVar] private Transform _target;

    [SerializeField] private GameObject _fireVFX;
    [SyncVar] GameObject _carWhoFiredMissile;
    [SyncVar] bool _exploded;

    private void Start()
    {
        _missileRigidBody = GetComponent<Rigidbody>();

        // Fire VFX
        //_fireVFX = Instantiate(_guidedMissileScriptableObject.MissileFireEffect, transform.position, Quaternion.identity);
        //_fireVFX.transform.forward = transform.forward;
    }

    void FixedUpdate()
    {
        if (!hasAuthority) return;
        MoveToTarget();
    }
    
    private void MoveToTarget()
    {
        if(_target != null)
        {
            // Get direction vector
            Vector3 direction = _target.position - transform.position;

            // Create rotation 
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Update rigidbody
            _missileRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _guidedMissileScriptableObject.MissileRotationSpeed * Time.fixedDeltaTime));
            _missileRigidBody.velocity = transform.forward * _guidedMissileScriptableObject.MissileSpeed;
        }
    }

    /// <summary>
    /// Sets the following values:
    /// - Target transform which the missile will travel towards.
    /// - Car from which we fired the missile. 
    /// </summary>
    /// <param name="target"></param>
    public void SetValues(Transform target, GameObject carWhoFiredMissile)
    {
        _target = target;
        _carWhoFiredMissile = carWhoFiredMissile;
    }

    [Command]
    private void Explode(Vector3 position)
    {
        _exploded = true;

        // Explosion VFX
        GameObject ExplosionEffect = Instantiate(_guidedMissileScriptableObject.MissileExplosionEffect, position, Quaternion.identity);
        NetworkServer.Spawn(ExplosionEffect);

        // Destroy objects
        RpcHideMissile();
        StartCoroutine(DestroyExplosion(ExplosionEffect, 5f));
    }

    [ClientRpc]
    private void RpcHideMissile()
    {
        _fireVFX.SetActive(false);
        gameObject.GetComponentInChildren<Renderer>().enabled = false;
    }

    private IEnumerator DestroyExplosion(GameObject explosion, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(explosion);
        NetworkServer.Destroy(explosion);
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAuthority) return;
        if (_exploded) return;
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) == 0) return;

        // If not colliding with itself
        if (_carWhoFiredMissile != other.GetComponent<Player>()?.Team.Vehicle.gameObject)
        {
            // If collided with enemy vehicle 
            if(other.CompareTag("Vehicle"))
            {
                // Apply damage to the vehicle we hit
                other.GetComponent<Player>().Team.Vehicle.gameObject.GetComponent<AttributeComponent>().CmdApplyDamage(_guidedMissileScriptableObject.MissileDamage);
            }

            Explode(transform.position);
        }
    }
}
