using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.Bus Master;
    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    public void ChangeMasterVolume(float value)
    {
        Master.setVolume(value);
    }
}
