using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject noSaveGameDialog; 
    [SerializeField] private GameObject settingsPanel
    ;

    GameState state;

    public void Start()
    {
        state = GameState.Instance;
    }
    public void onStartClick()
    {
        // reset save data
        state.currency = 0;
        state.weaponLevel = 0;
        state.dash = false;
        state.doubleJump = false;
        state.sceneName = "";
        state.savePointName = "";
        state.SaveState();

        // Load the game scene
        SceneManager.LoadScene("Level_One_Forrest");
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
        settingsPanel.SetActive(true);
        Debug.Log("Settings button clicked");
        //SceneManager.LoadScene("SettingScene");
        
    }

    public void onContinueClick()
    {
        // Load the last saved game
        // need to use game stat file to make sure the game is saved
        // then load the last saved game
        if (PlayerPrefs.HasKey("sceneName") && PlayerPrefs.HasKey("savePointName"))
        {
            state.RespawnPlayer();
        }
        else
        {
            noSaveGameDialog.SetActive(true);
            Debug.Log("No saved game found");
        }
        
    }
}
