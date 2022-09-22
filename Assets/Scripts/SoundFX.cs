using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private EventInstance instance;

    private bool _isBraking = false;

    private void Start()
    {
        instance = RuntimeManager.CreateInstance("event:/BrakeSfx");
    }

    public void PlayBrakeSfx()
    {
        if (!_isBraking)
        {
            _isBraking = true;
            instance.setParameterByName("IsBraking", 0);

            instance.start();
        }
    }

    public void StopBrakeSfx()
    {
        if (_isBraking)
        {
            _isBraking= false;
            instance.setParameterByName("IsBraking", 1);
        }
    }
}
