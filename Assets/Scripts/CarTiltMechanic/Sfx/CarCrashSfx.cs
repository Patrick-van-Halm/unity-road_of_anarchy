using FMOD.Studio;
using FMODUnity;
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
        if (!IsPlaying()) _eventInstance.start();
    }

    /// <summary>
    /// Sets the volume.
    /// </summary>
    /// <param name="volume">The volume amount (between 0-1).</param>
    public void SetSfxVolume(float volume)
    {
        if (!IsStarting() && !IsPlaying()) _eventInstance.setParameterByName("Volume", volume);
    }

    /// <returns>true if sfx is starting else false.</returns>
    private bool IsStarting()
    {
        _eventInstance.getPlaybackState(out PLAYBACK_STATE _playbackState);
        return _playbackState == PLAYBACK_STATE.STARTING;
    }

    /// <returns>true if sfx is playing else false.</returns>
    private bool IsPlaying()
    {
        _eventInstance.getPlaybackState(out PLAYBACK_STATE _playbackState);
        return _playbackState == PLAYBACK_STATE.PLAYING;
    }
}
