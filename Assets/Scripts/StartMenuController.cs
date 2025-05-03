using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void onStartClick()
    {
        // Load the game scene
        SceneManager.LoadScene("TestScene");
    }
    public void onQuitClick()
    {
        // Quit the game
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Stop playing the scene in the editor
#endif
        Application.Quit();
    }

    public void onSettingsClick()
    {
        // Load the settings scene
        // 
        SceneManager.LoadScene("SettingScene");
    }

    public void onContinueClick()
    {
        // Load the last saved game
        // need to use game stat file to make sure the game is saved
        // then load the last saved game
        Debug.Log("Continue button clicked");
    }
}
