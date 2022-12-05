using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapBehaviour : MonoBehaviour
{
    public Transform LockOn;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (!LockOn) return;
        Vector3 newPos = LockOn.position;
        transform.position = newPos + offset;

        transform.rotation = Quaternion.Euler(90, LockOn.eulerAngles.y, 0);
    }
}
