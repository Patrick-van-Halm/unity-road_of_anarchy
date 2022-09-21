using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueTo : NetworkBehaviour
{
    [SyncVar] public Transform Target;
    [SyncVar] public Vector3 LocalPosition;

    private void Update()
    {
        transform.position = Target.position + LocalPosition;
        transform.rotation = Target.rotation;
    }
}
