using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameState : MonoBehaviour
{

    public static GameState Instance;

    // References
    public Player player;

    // Logic
    public int currency;

    /*
     * Unlocked character upgrades
     */
    public bool dash;
    public bool doubleJump;

    /*
     * save respawn point (scene #, respawn point name)
     */
    public string sceneName;
    public string savePointName;

    // remember what the last scene the player was in
    private string previousSceneName;
    private Vector3 levelTwoSecondarySpawn = new Vector3(57, -1.5f, 0);
    private Vector3 levelOneSecondarySpawn = new Vector3(50, -0.45f, 0);

    /*
     *  Called when script is loaded
     */
    private void Awake()
    {
        if (GameState.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadState();
    }

    /*
     * INT currency
     * INT dash
     * INT doubleJUmp
     * INT sceneNumber
     * STRING savePointName
     */
    public void SaveState()
    {
        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.SetInt("dash", dash ? 1 : 0);
        PlayerPrefs.SetInt("doubleJump", doubleJump ? 1 : 0);
        PlayerPrefs.SetString("sceneName", sceneName);
        PlayerPrefs.SetString("savePointName", savePointName);
        PlayerPrefs.SetString("previousSceneName", SceneManager.GetActiveScene().name);
    }

    public void LoadState()
    {
        currency = PlayerPrefs.GetInt("currency", 0);
        dash = PlayerPrefs.GetInt("dash", 0) == 1;
        doubleJump = PlayerPrefs.GetInt("doubleJump", 0) == 1;

        // for saving respawn point
        sceneName = PlayerPrefs.GetString("sceneName", "StartScene");
        savePointName = PlayerPrefs.GetString("savePointName", "");

        // for knowing what part of the scene to load player into
        previousSceneName = PlayerPrefs.GetString("previousSceneName", "");
    }

    /*
     * called when script is loaded
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadState();
        player = GameObject.Find("Player").GetComponent<Player>();
        // handle starting position for player based on where player enters level from
        if (scene.name == "Level_Two_Town")
        {
            if (previousSceneName == "Level_Three_Cave")
            {
                player.transform.position = levelTwoSecondarySpawn;

            }
        }

        if (scene.name == "Level_One_Forrest")
        {
            if (previousSceneName == "Level_Two_Town")
            {
                player.transform.position = levelOneSecondarySpawn;

            }
        }

    }

    private void OnApplicationQuit()
    {
        SaveState();
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(sceneName);
        // After scene loads, place player at `savePointName` location
    }


    // ===== SETTER FUNCTIONS =====
    public void UnlockDash()
    {
        dash = true;
        PlayerPrefs.SetInt("dash", 1);
    }

    public void UnlockDoubleJump()
    {
        doubleJump = true;
        PlayerPrefs.SetInt("doubleJump", 1);
    }

    public void SetRespawnPoint(string savePointName = "")
    {
        // set respawn point scene locally & persistently
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        sceneName = SceneManager.GetActiveScene().name;

        // set respawn point position locally & persistently
        PlayerPrefs.SetString("savePointName", savePointName);
        this.savePointName = savePointName;
    }

    public void AddCurrency(int amt)
    {
        currency += amt;
        PlayerPrefs.SetInt("currency", currency);
    }

    public string GetPreviousScene()
    {
        return previousSceneName;
    }
}
