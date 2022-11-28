using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CarWheelsGroundedSfx : MonoBehaviour
{
    private EventInstance _eventInstance;

    private void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance("event:/WheelsGroundedSfx");
    }

    private void Update()
    {
        _eventInstance.set3DAttributes(transform.To3DAttributes());
    }

    /// <summary>
    /// Starts the sound.
    /// </summary>
    public void ActivateInstance()
    {
        if (!IsPlaying()) _eventInstance.start();
    }

    /// <returns>true if sfx is playing else false.</returns>
    private bool IsPlaying()
    {
        _eventInstance.getPlaybackState(out PLAYBACK_STATE _playbackState);
        return _playbackState == PLAYBACK_STATE.PLAYING;
    }
}
