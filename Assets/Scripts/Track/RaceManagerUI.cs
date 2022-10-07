using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceManagerUI : MonoBehaviour
{
    [SerializeField] private RaceManager _raceManager;

    [Header("Text")]
    [SerializeField] private GameObject _wrongWayTextObject;
    [SerializeField] private GameObject _lapTextObject;
    [SerializeField] private GameObject _positionTextObject;

    private TMP_Text _lapText;
    private TMP_Text _positionText;
    private int _numberOfLaps; 

 
    void Start()
    {
        _lapText = _lapTextObject.GetComponent<TMP_Text>();
        _positionText = _positionTextObject.GetComponent<TMP_Text>();

        _raceManager.WrongCheckpoint.AddListener(WrongCheckpoint);
        _raceManager.CorrectCheckpoint.AddListener(CorrectCheckpoint);
        _raceManager.SetNumberOfLaps.AddListener(SetNumberOfLaps);
        _raceManager.IncreaseLap.AddListener(IncreaseLap);
        _raceManager.OnPositionUpdate.AddListener(OnPositionUpdate);
        _raceManager.OnSwapPosition.AddListener(OnSwapPosition);
    }

    private void WrongCheckpoint()
    {
        _wrongWayTextObject.SetActive(true);
    }

    private void CorrectCheckpoint()
    {
        _wrongWayTextObject.SetActive(false);
    }

    private void SetNumberOfLaps(int numberOfLaps)
    {
        _numberOfLaps = numberOfLaps;
        _lapText.text = "1 / " + numberOfLaps.ToString();
    }

    private void IncreaseLap(int lap)
    {
        int tmp = lap + 1;
        _lapText.text = tmp.ToString() + " / " + _numberOfLaps.ToString();
    }

    private void OnPositionUpdate(int position)
    {
        _positionText.text = position.ToString();
    }

    private void OnSwapPosition(int position)
    {
        _positionText.text = position.ToString();
    }
}
