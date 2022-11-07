using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueTo : NetworkBehaviour
{
    [SyncVar] public Transform Target;
    [SyncVar] public Vector3 LocalPosition;
    [SyncVar] private Vector3 pos, fw, up;

    private void Start()
    {
        if (!isServer) return;
        transform.position = Target.position + LocalPosition;
        pos = Target.transform.InverseTransformPoint(transform.position);
        fw = Target.transform.InverseTransformDirection(transform.forward);
        up = Target.transform.InverseTransformDirection(transform.up);
    }

    private void Update()
    {
        if (Target == null || transform == null) return;
        var newpos = Target.transform.TransformPoint(pos);
        var newfw = transform.TransformDirection(fw);
        var newup = Target.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;
    }
}
