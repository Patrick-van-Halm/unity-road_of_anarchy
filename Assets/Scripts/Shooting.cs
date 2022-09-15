using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunEndPointPosition;
    public GameObject bullet;
    public AudioSource gunAudio;
    public string vehicleTag;

    [Header("Weapon Settings")]
    public int weaponDamage;
    public int weaponRange;

    [Header("Bullet Settings")]
    public int bulletSpeed;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
            InstantiateBullet();
            PlayAudio();
        }
    }

    private void FireGun()
    {
        // Check if vehicle gets shot
        RaycastHit hit;
        if (Physics.Raycast(gunEndPointPosition.transform.position, gunEndPointPosition.transform.forward, out hit, weaponRange))
        {
            if (hit.transform.CompareTag(vehicleTag))
            {
                Debug.Log("Hit: " + hit.transform.tag);
            }
        }
    }

    private void InstantiateBullet()
    {
        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, gunEndPointPosition.transform.position, Quaternion.identity);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = gunEndPointPosition.transform.forward;

        // Add script to bullet and set the speed
        currentBullet.AddComponent<Bullet>().bulletSpeed = bulletSpeed;
    }

    private void PlayAudio()
    {
        // Play audio
        //gunAudio.Play();
    }
}
