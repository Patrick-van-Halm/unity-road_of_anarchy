using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCrashSfx : MonoBehaviour
{
    private EventInstance _eventInstance;

    private void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance("event:/CrashSfx");
    }

    /// <summary>
    /// Starts the sound.
    /// </summary>
    public void ActivateInstance()
    {
        _eventInstance.start();
    }

    /// <summary>
    /// Sets the volume.
    /// </summary>
    /// <param name="volume">The volume amount (between 0-1).</param>
    public void SetSfxVolume(float volume)
    {
        _eventInstance.setParameterByName("Volume", volume);
    }
}
