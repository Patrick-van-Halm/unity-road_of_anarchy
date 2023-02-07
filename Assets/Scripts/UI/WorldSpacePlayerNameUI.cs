using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpacePlayerNameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _TMPDriverName;
    [SerializeField] private TMP_Text _TMPGunnerName;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float maximumDistanceForFade;

    [Header("Game Settings")]
    [SerializeField] private GameSettings _gameSettings;

    private void Update()
    {
        FadeUI();
    }

    private void FadeUI()
    {
        if (!NetworkClient.active) return;
        float distance = Vector3.Distance(transform.position, NetworkClient.connection.identity.transform.position);
        float fading = (maximumDistanceForFade - distance) / maximumDistanceForFade;
        _canvasGroup.alpha = Mathf.Clamp01(fading);
    }

    public void SetDriverName(string name)
    {
        _TMPDriverName.text = name;
    }

    public void SetGunnerName(string name)
    {
        _TMPGunnerName.text = name;
    }
}
