using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSFX : MonoBehaviour
{
    [Header("Studio event emitter")]
    [SerializeField] private StudioEventEmitter _eHover;

    private EventInstance _instanceHover;
    private void Start()
    {
        _instanceHover = RuntimeManager.CreateInstance("event:/HoverSfx");
    }

    public void PlayHoverSound(float pitch)
    {
        _instanceHover.setPitch(pitch);
        _instanceHover.start();
    }
}
