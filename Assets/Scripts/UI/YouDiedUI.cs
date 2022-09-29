using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouDiedUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EventInstance _fadeinUISound;

    private void Awake()
    {
        _fadeinUISound = RuntimeManager.CreateInstance("event:/EliminatedSfx");
    }

    private void OnEnable()
    {
        _animator.SetTrigger("FadeIn");
        _fadeinUISound.start();
    }

    public void Disconnect()
    {
        FadeOut();
    }

    private void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }

    public void Disable ()
    {
        gameObject.SetActive(false);
        GameNetworkManager.Instance?.Disconnect();
    }
}
