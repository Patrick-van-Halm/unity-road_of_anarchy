using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSetter : MonoBehaviour
{
    [Header("Speed = value 0 - 100")]
    public float _speed;

    [Header("Studio event emitters")]
    [SerializeField] private StudioEventEmitter _eEngine;
    [SerializeField] private StudioEventEmitter _eBraking;

    private EventInstance _brakeInstance;
    private bool _isBraking = false;

    private void Start()
    {
        _brakeInstance = RuntimeManager.CreateInstance("event:/BrakeSfx");
    }

    void Update()
    {
        _eEngine.SetParameter("Speed", _speed);
    }

    public void PlayBrakeSFX()
    {
        if (!_isBraking)
        {
            _isBraking = true;
            _brakeInstance.setParameterByName("IsBraking", 0);

            _brakeInstance.start();
        }
    }
    public void StopBrakeSFX()
    {
        if (_isBraking)
        {
            _isBraking = false;
            _brakeInstance.setParameterByName("IsBraking", 1);
        }
    }
}
