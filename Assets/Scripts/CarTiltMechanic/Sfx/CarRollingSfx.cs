using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CarRollingSfx : MonoBehaviour
{
    private EventInstance _eventInstance;

    private void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance("event:/RollingSfx");
    }

    /// <summary>
    /// Starts the sound.
    /// </summary>
    public void ActivateInstance()
    {
        _eventInstance.setParameterByName("IsRolling", 1);
        if (!IsPlaying()) _eventInstance.start();
    }

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public void DeactivateInstance()
    {
        _eventInstance.setParameterByName("IsRolling", 0);
    }

    /// <returns>true if sfx is playing else false.</returns>
    private bool IsPlaying()
    {
        _eventInstance.getPlaybackState(out PLAYBACK_STATE _playbackState);
        return _playbackState == PLAYBACK_STATE.PLAYING;
    }
}
