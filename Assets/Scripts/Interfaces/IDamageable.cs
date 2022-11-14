using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(float value);

    [Command]
    void CmdApplyDamage(float value, NetworkConnectionToClient connectionToClient = null);
}
