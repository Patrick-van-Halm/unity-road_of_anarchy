using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform weaponEndPointPosition;
    public Transform weapon;
    public Camera gunnerCamera;
    public GameObject bullet;
    public AudioSource weaponAudio;
    public string vehicleTag;

    [Header("Weapon Settings")]
    public int weaponDamage;
    public int weaponRange;

    [Header("Bullet Settings")]
    public int bulletSpeed;

    private Vector3 bulletDirection; 
    private RaycastHit hit;
    private bool hasHit;


    void Update()
    {
        RotateWeapon();

        // When firing weapon
        if (Input.GetMouseButtonDown(0))
        {
            InstantiateBullet();
            PlayAudio();

            // Hit enemy vehicle
            if(hasHit)
            {
                Debug.Log("Hit enemy vehicle");
            }
        }
    }

    private void RotateWeapon()
    {
        // Raycast in the middle of the camera
        Ray ray = gunnerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Check if ray hits something, than store the hitpoint of the ray
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            // Check for hit on an enemy vehicle
            if (hit.transform.CompareTag(vehicleTag))
            {
                // Hit enemy vehicle
                hasHit = true;
            }

            // Set hit point
            targetPoint = hit.point;
        }
        else
        {
            // Not hit enemy vehicle
            hasHit = false;

            // Ray has not hit anything. Just shoot straight ahead stopping at weapon range
            targetPoint = ray.GetPoint(weaponRange);
        }

        // Calculate the direction in which the weapon needs to fire
        bulletDirection = targetPoint - weaponEndPointPosition.transform.position;

        // Rotate weapon to shoot direction
        weapon.transform.forward = bulletDirection.normalized;
    }

    private void InstantiateBullet()
    {
        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, weaponEndPointPosition.transform.position, Quaternion.identity);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = bulletDirection.normalized;

        // Add script to bullet and set the speed
        currentBullet.AddComponent<Bullet>().bulletSpeed = bulletSpeed;
    }

    private void PlayAudio()
    {
        // Play audio
        //weaponAudio.Play();
    }
}