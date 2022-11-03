using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceManagerUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private GameObject _wrongWayTextObject;
    [SerializeField] private GameObject _lapTextObject;
    [SerializeField] private GameObject _positionTextObject;

    private TMP_Text _lapText;
    private TMP_Text _positionText;

    public void RaceManagerReady()
    {
        if(_lapTextObject) _lapText = _lapTextObject.GetComponent<TMP_Text>();
        _positionText = _positionTextObject.GetComponent<TMP_Text>();

        RaceManager.Instance.WrongCheckpoint.AddListener(WrongCheckpoint);  
        RaceManager.Instance.CorrectCheckpoint.AddListener(CorrectCheckpoint);
        if(_lapTextObject) RaceManager.Instance.IncreaseLap.AddListener(IncreaseLap);
        RaceManager.Instance.OnPositionUpdate.AddListener(OnPositionUpdate);

        _lapText.text = "1 / " + RaceManager.Instance.NumberOfLaps.ToString();
    }

    private void WrongCheckpoint()
    {
        _wrongWayTextObject.SetActive(true);
    }

    private void CorrectCheckpoint()
    {
        _wrongWayTextObject.SetActive(false);
    }

    private void IncreaseLap(int lap)
    {
        int tmp = lap + 1;
        _lapText.text = tmp.ToString() + " / " + RaceManager.Instance.NumberOfLaps.ToString();
    }

    private void OnPositionUpdate(int position)
    {
        string positionText;
        switch (position)
        {
            case 1:
                positionText = "1ST";
                break;

            case 2:
                positionText = "2ND";
                break;

            case 3:
                positionText = "3RD";
                break;

            default:
                positionText = $"{position}TH";
                break;
        }
        _positionText.text = positionText;
    }
}
