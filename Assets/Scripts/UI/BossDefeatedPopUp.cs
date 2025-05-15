using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDefeatedPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartSceneButtonClicked()
    {
        // Load the next scene or perform any action you want
        // For example, load the main menu or the next level
        SceneManager.LoadScene("StartScene");
    }

    public void OnExitButtonClicked()
    {
        // Exit the game
        Application.Quit();
        Debug.Log("Game Exited");
    }
}
