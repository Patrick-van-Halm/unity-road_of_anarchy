using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWater : MonoBehaviour
{
    [SerializeField] private NewKartScript _kartScript;
    [SerializeField] private Vehicle vehicle;

    private EventInstance _instanceWater;

    private bool _inWater = false;

    private void Start()
    {
        _instanceWater = RuntimeManager.CreateInstance("event:/WaterSplash");
        vehicle?.OnInWater.AddListener(PlayWaterSoundInWater);
    }

    private void PlayWaterSoundInWater(bool isInWater)
    {
        if (isInWater) _instanceWater.start();
        else _instanceWater.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Update()
    {
        _instanceWater.set3DAttributes(_kartScript.transform.To3DAttributes());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!vehicle.isLocalPlayer) return;
        if (other.gameObject.tag == "Water")
        {
            _inWater = true;

            _kartScript.InWater(_inWater);
            vehicle?.InWater(_inWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!vehicle.isLocalPlayer) return;
        if (other.gameObject.tag == "Water")
        {
            _inWater = false;

            _kartScript.InWater(_inWater);
            vehicle?.InWater(_inWater);
        }
    }
}
