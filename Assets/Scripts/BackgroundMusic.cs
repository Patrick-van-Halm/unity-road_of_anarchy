using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static EventInstance _eventInstance;
    private static bool _musicPlaying;

    [SerializeField] private Lobby _lobby;

    private void Start()
    {
        if (_lobby != null) _lobby.OnLobbyStarted.AddListener(StopBackgroundMusic);
        if (!_musicPlaying)
        {
            _eventInstance = RuntimeManager.CreateInstance("event:/BackgroundMusic");
            _eventInstance.start();
            _eventInstance.release();
            _musicPlaying = true;
        }
    }

    private void StopBackgroundMusic()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicPlaying = false;
    }
}
