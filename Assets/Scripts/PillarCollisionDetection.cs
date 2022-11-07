using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PillarCollisionDetection : MonoBehaviour
{
    public UnityEvent OnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if wall is hit
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player")) OnCollision?.Invoke();
    }
}
