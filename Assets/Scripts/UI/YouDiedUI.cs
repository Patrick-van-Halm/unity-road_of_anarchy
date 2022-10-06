using FMOD.Studio;
using FMODUnity;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YouDiedUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _button;

    private EventInstance _fadeinUISound;

    private void Awake()
    {
        _fadeinUISound = RuntimeManager.CreateInstance("event:/EliminatedSfx");
    }

    private void Start()
    {
        if (NetworkServer.active) _button.interactable = false;
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
