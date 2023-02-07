using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private RawImage[] _countdownImages;

    [SerializeField] private float _fadeOutMultiplier = 0.5f;
    
    // Start is called before the first frame update
    private void Start()
    {
        DisableCountdownImages();
    }

    public void UpdateCountdownUI(int number)
    {
        DisableCountdownImages();
        _countdownImages[number].enabled = true;
        _countdownImages[number].GetComponent<FMODUnity.StudioEventEmitter>().Play();
        if (number == 0) FadeCountdown();
    }

    private void DisableCountdownImages()
    {
        foreach (RawImage image in _countdownImages)
        {
            image.enabled = false;
        }
    }

    private void FadeCountdown()
    {
        StartCoroutine(CoroFadeMessage());
    }

    private IEnumerator CoroFadeMessage()
    {
        while (_canvasGroup.alpha != 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * _fadeOutMultiplier;
            yield return null;
        }
    }
}
