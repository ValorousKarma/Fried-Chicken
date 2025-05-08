using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPaused= false;
    public GameObject PauseMenu;
    private void Awake()
    {
        // Ensure the pause menu is not active at the start
        DontDestroyOnLoad(PauseMenu);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            
        }
        
    }
    void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Settings()
    {
        // Implement settings logic here
        Debug.Log("Settings Opened");
    }

    public void SaveGame()
    {
        // Implement save game logic here
        Debug.Log("Game Saved");
    }

    public void quitGame()
    {
        // Implement quit game logic here
        Debug.Log("Game Quit");
        Application.Quit();
    }

   
}
