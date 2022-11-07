using UnityEngine;

public class CheckpointBackCollider : MonoBehaviour
{
    public bool Entered { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Transform car = other.transform;

        // When vehicle enters collider
        if (car != null && car.CompareTag("Player")) Entered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Transform car = other.transform;

        // When vehicle exits collider
        if (car != null && car.CompareTag("Player")) Entered = false;
    }
}
