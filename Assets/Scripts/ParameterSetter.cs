using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSetter : MonoBehaviour
{
    [Header("Speed = value 0 - 100")]
    public float _speed;

    private StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        emitter.SetParameter("Speed", _speed);
    }
}
