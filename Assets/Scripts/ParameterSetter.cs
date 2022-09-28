using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class ParameterSetter : MonoBehaviour
{
    [SerializeField] private VehicleAudioSync audioSync;

    [Header("Studio event emitters")]
    [SerializeField] private StudioEventEmitter _eEngine;
    [SerializeField] private StudioEventEmitter _eBraking;

    private EventInstance _brakeInstance;

    private void Start()
    {
        _brakeInstance = RuntimeManager.CreateInstance("event:/BrakeSfx");
    }

    void Update()
    {
        _eEngine.SetParameter("Speed", audioSync.Speed);
        UpdateBraking();
    }

    private void UpdateBraking()
    {
        // Check if already braking so we don't keep starting sound
        _brakeInstance.getParameterByName("IsBraking", out float isBraking);
        if (isBraking == 1 && audioSync.IsBraking)
        {
            // Reset isBraking parameter so braking doesn't break out of the loop
            _brakeInstance.setParameterByName("IsBraking", 0);
            _brakeInstance.start();
        }
        else if (isBraking == 0 && !audioSync.IsBraking)
        {
            // Apply isBraking parameter so it breaks out of the breaking loop
            _brakeInstance.setParameterByName("IsBraking", 1);
        }
    }

    public void SetSpeed(float speed)
    {
        audioSync.SetSpeed(speed);
    }

    /// <summary>
    /// Toggles the braking if it isn't braking yet
    /// </summary>
    public void PlayBrakeSFX()
    {
        if (!audioSync.IsBraking) ToggleBraking();
    }

    /// <summary>
    /// Toggles the braking if braking is applied
    /// </summary>
    public void StopBrakeSFX()
    {
        if (audioSync.IsBraking) ToggleBraking();
    }

    /// <summary>
    /// Toggle the braking
    /// </summary>
    public void ToggleBraking()
    {
        audioSync.SetIsBraking(!audioSync.IsBraking);
    }
}
