using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueTo : NetworkBehaviour
{
    [SyncVar] public Transform Target;
    [SyncVar] public Vector3 LocalPosition;
    private Vector3 pos, fw, up;

    private void Start()
    {
        transform.position = Target.position + LocalPosition;
        pos = Target.transform.InverseTransformPoint(transform.position);
        fw = Target.transform.InverseTransformDirection(transform.forward);
        up = Target.transform.InverseTransformDirection(transform.up);
    }

    private void Update()
    {
        var newpos = Target.transform.TransformPoint(pos);
        var newfw = Target.transform.TransformDirection(fw);
        var newup = Target.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;
    }
}
