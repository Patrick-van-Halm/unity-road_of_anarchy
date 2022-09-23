using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFallingSfx : MonoBehaviour
{
    private EventInstance _eventInstance;

    private void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance("event:/FallingSfx");
    }

    /// <summary>
    /// Starts the sound.
    /// </summary>
    public void ActivateInstance()
    {
        _eventInstance.setParameterByName("IsFalling", 0);
        _eventInstance.setParameterByName("Volume", 0);
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

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void DeactivateInstance()
    {
        _eventInstance.setParameterByName("IsFalling", 1);
    }
}
