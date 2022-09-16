using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Profiling.RawFrameDataView;

public class ParameterSetter : MonoBehaviour
{
    [Header("Speed = value 0 - 100")]
    public float _speed;

    void Update()
    {
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("Speed", _speed);
    }
}
