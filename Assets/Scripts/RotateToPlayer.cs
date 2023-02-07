using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    public Transform CamTransform;

    void Update()
    {
        if (!CamTransform) return;
        transform.rotation = Quaternion.Euler(0, CamTransform.eulerAngles.y, 0);
    }
}
