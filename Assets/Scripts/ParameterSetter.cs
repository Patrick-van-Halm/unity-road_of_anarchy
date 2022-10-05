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
    private bool _isBraking;
    private float _speed;

    private void Start()
    {
        _instanceBraking = RuntimeManager.CreateInstance("event:/BrakeSfx");
        _instanceGunMove = RuntimeManager.CreateInstance("event:/MechanicalSfx");
    }

    void Update()
    {
        //_eEngine.SetParameter("Speed", audioSync ? audioSync.Speed : _speed);
        //UpdateBraking();
        UpdateGunMoving();
    }

    private void UpdateBraking()
    {
        // Check if already braking so we don't keep starting sound
        _instanceBraking.getParameterByName("IsBraking", out float isBraking);
        if (isBraking == 1 && audioSync ? audioSync.IsBraking : _isBraking)
        {
            // Reset isBraking parameter so braking doesn't break out of the loop
            _instanceBraking.setParameterByName("IsBraking", 0);
            _instanceBraking.start();
        }
        else if (isBraking == 0 && audioSync ? !audioSync.IsBraking : _isBraking)
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
        if(audioSync) audioSync.SetSpeed(speed);
        _speed = speed;
    }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayBrakeSFX() 
    { 
        if ((audioSync && !audioSync.IsBraking) || !_isBraking) ToggleBraking(); 
    }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopBrakeSFX() 
    { 
        if ((audioSync && audioSync.IsBraking) || _isBraking) ToggleBraking(); 
    }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    public void ToggleBraking() 
    { 
        if(audioSync) audioSync.SetIsBraking(!_isBraking);
        _isBraking = !_isBraking;
    }

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
