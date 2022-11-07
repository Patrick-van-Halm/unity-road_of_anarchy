using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWater : MonoBehaviour
{
    [SerializeField] private NewKartScript _kartScript;

    private EventInstance _instanceWater;

    private void Start()
    {
        _instanceWater = RuntimeManager.CreateInstance("event:/WaterSplash");
    }

    private void Update()
    {
        _instanceWater.set3DAttributes(_kartScript.transform.To3DAttributes());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            _instanceWater.start();
            _kartScript.InWater(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water") _kartScript.InWater(false);
    }
}
