using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject SettingsPanel;

    public void GoToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void TogglePanel(GameObject Panel)
    {
        if (!Panel.activeInHierarchy)
        {
            Panel.SetActive(true); return;
        }
            Panel.SetActive(false);
    }

    public void HideCursor()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
