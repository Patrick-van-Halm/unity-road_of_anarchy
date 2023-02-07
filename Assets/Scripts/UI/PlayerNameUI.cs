using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text TMPDriverName;
    [SerializeField] private TMP_Text TMPGunnerName;

    [Header("Game Settings")]
    [SerializeField] private GameSettings _gameSettings;

    public void SetDriverName(string name)
    {
        TMPDriverName.text = name;
    }

    public void SetGunnerName(string name)
    {
        TMPGunnerName.text = name;
    }
}
