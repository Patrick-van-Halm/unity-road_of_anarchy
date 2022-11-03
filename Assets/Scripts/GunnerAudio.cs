using FMOD;
using FMOD.Studio;
using FMODUnity;
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
    }

    void Update()
    {
        _instanceGunMove.set3DAttributes(transform.To3DAttributes());

        UpdateGunMoving();
    }

    private void UpdateGunMoving()
    {
        // Check if already moving so we don't keep starting sound
        _instanceGunMove.getParameterByName("IsMoving", out float isGunMoving);
        if (isGunMoving == 1 && audioSync ? audioSync.IsMoving : _isMoving)
        {
            // Reset isMoving parameter so moving doesn't break out of the loop
            _instanceGunMove.setParameterByName("IsMoving", 0);
            _instanceGunMove.start();
        }
        else if (isGunMoving == 0 && audioSync ? !audioSync.IsMoving : !_isMoving)
        {
            // Apply movingsMoving parameter so it breaks out of the breaking loop
            _instanceGunMove.setParameterByName("IsMoving", 1);
        }
    }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayIsMovingSFX() { if ((audioSync != null && !audioSync.IsMoving) || !_isMoving) ToggleIsMoving(); }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopIsMovingSFX() { if ((audioSync != null && audioSync.IsMoving) || _isMoving) ToggleIsMoving(); }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    public void ToggleIsMoving() 
    { 
        if(audioSync) audioSync.SetIsMoving(!_isMoving);
        _isMoving = !_isMoving;
    }
}
