using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Scripting;

public class ParameterSetter : MonoBehaviour
{
    [SerializeField] private VehicleAudioSync audioSync;

    [Header("Studio event emitters")]
    [SerializeField] private StudioEventEmitter _eEngine;
    [SerializeField] private StudioEventEmitter _eBraking;

    private EventInstance _instanceBraking;
    private EventInstance _instanceGunMove;

    private bool _isMoving;

    private void Start()
    {
        _instanceBraking = RuntimeManager.CreateInstance("event:/BrakeSfx");
        _instanceGunMove = RuntimeManager.CreateInstance("event:/MechanicalSfx");
    }

    void Update()
    {
        //_eEngine.SetParameter("Speed", audioSync.Speed);
        //UpdateBraking();
        UpdateGunMoving();
    }

    private void UpdateBraking()
    {
        // Check if already braking so we don't keep starting sound
        _instanceBraking.getParameterByName("IsBraking", out float isBraking);
        if (isBraking == 1 && audioSync.IsBraking)
        {
            // Reset isBraking parameter so braking doesn't break out of the loop
            _instanceBraking.setParameterByName("IsBraking", 0);
            _instanceBraking.start();
        }
        else if (isBraking == 0 && !audioSync.IsBraking)
        {
            // Apply isBraking parameter so it breaks out of the breaking loop
            _instanceBraking.setParameterByName("IsBraking", 1);
        }
    }

    private void UpdateGunMoving()
    {
        // Check if already moving so we don't keep starting sound
        _instanceGunMove.getParameterByName("IsMoving", out float isGunMoving);
        if (isGunMoving == 1 && _isMoving)
        {
            print("Here");
            // Reset isMoving parameter so moving doesn't break out of the loop
            _instanceGunMove.setParameterByName("IsMoving", 0);
            _instanceGunMove.start();
        }
        else if (isGunMoving == 0 && !_isMoving)
        {
            // Apply movingsMoving parameter so it breaks out of the breaking loop
            _instanceGunMove.setParameterByName("IsMoving", 1);
        }
    }

    public void SetSpeed(float speed)
    {
        audioSync.SetSpeed(speed);
    }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayBrakeSFX() { if (!audioSync.IsBraking) ToggleBraking(); }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopBrakeSFX() { if (audioSync.IsBraking) ToggleBraking(); }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    public void ToggleBraking() { audioSync.SetIsBraking(!audioSync.IsBraking); }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayIsMovingSFX() { _isMoving = true; }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopIsMovingSFX() { _isMoving = false; }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    //public void ToggleIsMoving() { audioSync.SetIsMoving(!_isMoving); }
}
