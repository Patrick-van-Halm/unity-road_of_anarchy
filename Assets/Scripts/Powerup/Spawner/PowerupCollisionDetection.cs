using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerupCollisionDetection : MonoBehaviour
{
    public UnityEvent<Team> HasCollided = new UnityEvent<Team>();

    private void OnTriggerEnter(Collider collidedObj)
    {
        // Check if the powerup is hit
        if (collidedObj.gameObject.CompareTag("Player")) HasCollided?.Invoke(collidedObj.GetComponent<Player>().Team);
    }
}
