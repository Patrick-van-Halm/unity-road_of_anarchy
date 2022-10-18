using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarRotate : NetworkBehaviour
{
    [SerializeField] private PillarCollisionDetection _pillarCollisionDetection;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _pillarCollisionDetection.OnCollision.AddListener(CmdStartAnim);
    }

    [Command(requiresAuthority = false)]
    private void CmdStartAnim()
    {
        RpcStartAnim();
    }

    [ClientRpc]
    private void RpcStartAnim()
    {
        if (!_animator.GetBool("AnimActive")) _animator.SetBool("AnimActive", true);
    }

    public void AnimEnded()
    {
        _animator.SetBool("AnimActive", false);
        _animator.SetBool("IsNeg", !_animator.GetBool("IsNeg"));
    }
}
