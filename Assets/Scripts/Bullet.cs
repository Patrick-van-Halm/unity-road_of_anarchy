using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;

    IEnumerator Start() 
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void Update()
    {
        // Add movement to bullet
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }
}
