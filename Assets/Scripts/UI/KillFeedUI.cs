using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillFeedUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private TMP_Text _text;
    
    public TMP_Text Text { get { return _text; } }
    [SerializeField] private WreckedCar _wreckedCar;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _text = GetComponentInChildren<TMP_Text>();
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        _wreckedCar.KillFeed.AddListener(ShowMessage);
    }

    public void ShowMessage(string text)
    {
        _text.text = text;
        _canvasGroup.alpha = 1f;
        StartCoroutine(CoroFadeMessage());
    }

    private IEnumerator CoroFadeMessage()
    {
        while (_canvasGroup.alpha != 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * 0.2f; 
            yield return null;
        }
    }
}
