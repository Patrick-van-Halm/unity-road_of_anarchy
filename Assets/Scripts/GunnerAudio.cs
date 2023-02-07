using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Scripting;

public class GunnerAudio : MonoBehaviour
{
    [SerializeField] private GunnerAudioSync audioSync;
    private EventInstance _instanceGunMove;

    private bool _isMoving;

    private void Start()
    {
        _instanceGunMove = RuntimeManager.CreateInstance("event:/MechanicalSfx");

        if (audioSync != null && !audioSync.isLocalPlayer) audioSync.OnIsMovingChanged.AddListener(SetIsMoving);
    }

    void Update()
    {
        _instanceGunMove.set3DAttributes(transform.To3DAttributes());
    }

    private void OnDisable()
    {
        StopIsMovingSFX();
    }

    private void UpdateGunMoving()
    {
        // Check if already moving so we don't keep starting sound
        _instanceGunMove.getParameterByName("IsMoving", out float isGunMoving);
        if (isGunMoving == 1 && _isMoving)
        {
            // Reset isMoving parameter so moving doesn't break out of the loop
            _instanceGunMove.setParameterByName("IsMoving", 0);
            _instanceGunMove.start();
            print("moving");
        }
        else if (isGunMoving == 0 && !_isMoving)
        {
            // Apply movingsMoving parameter so it breaks out of the breaking loop
            _instanceGunMove.setParameterByName("IsMoving", 1);
            print("stopped moving");
        }
    }

    private void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
        UpdateGunMoving();
    }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayIsMovingSFX() { if (!_isMoving) ToggleIsMoving(); }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopIsMovingSFX() { if (_isMoving) ToggleIsMoving(); }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    public void ToggleIsMoving() 
    { 
        _isMoving = !_isMoving;
        if(audioSync) audioSync.SetIsMoving(!_isMoving);
        UpdateGunMoving();
    }
}
