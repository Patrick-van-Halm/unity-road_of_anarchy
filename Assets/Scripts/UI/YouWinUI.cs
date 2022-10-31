using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YouWinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void ShowWinUI(string text)
    {
        Debug.Log("Joehoe");
        _text.text = text;
    }
}
