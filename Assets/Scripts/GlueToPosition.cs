using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueToPosition : MonoBehaviour
{
    public Transform Target;
    public Vector3 LocalPosition;
    private Vector3 pos;

    private void Start()
    {
        transform.position = Target.position + LocalPosition;
        pos = Target.transform.InverseTransformPoint(transform.position);
    }

    private void Update()
    {
        if (Target == null || transform == null) return;
        var newpos = Target.transform.TransformPoint(pos);
        transform.position = newpos;
    }
}
