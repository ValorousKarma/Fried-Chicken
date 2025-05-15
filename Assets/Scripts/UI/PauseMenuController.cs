using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    private SceneManager sceneManager;

    private static PauseMenuController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This keeps the entire manager alive
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates across scenes
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.OnGamePaused();
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.OnGameResumed();
        isPaused = false;
    }

    public void Settings()
    {
        Debug.Log("Settings Opened");
    }

    public void SaveGame()
    {
       SceneManager.LoadScene("StartScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
