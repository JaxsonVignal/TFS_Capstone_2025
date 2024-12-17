using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Unlock and show the cursor at the start of the game
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;                   // Make the cursor visible
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("TestLevel");
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif

    }
}
