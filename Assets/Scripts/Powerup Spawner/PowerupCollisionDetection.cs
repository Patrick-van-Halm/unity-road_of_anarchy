using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerupCollisionDetection : MonoBehaviour
{
    public UnityEvent HasCollided = new UnityEvent();

    private void OnTriggerEnter(Collider collidedObj)
    {
        // Check if the powerup is hit
        if (collidedObj.gameObject.CompareTag("Player")) HasCollided?.Invoke();
    }
}
