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
    public struct Upgrades 
    {
        public bool dash;
        public bool doubleJump;
    }

    // save respawn point (scene #, respawn point name)
    public struct RespawnPoint
    {
        public string sceneName;
        public string savePointName;
    }

    Upgrades upgrades;
    RespawnPoint respawnPoint;

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
        PlayerPrefs.SetInt("dash", upgrades.dash ? 1 : 0);
        PlayerPrefs.SetInt("doubleJump", upgrades.doubleJump ? 1 : 0);
        PlayerPrefs.SetString("sceneName", respawnPoint.sceneName);
        PlayerPrefs.SetString("savePointName", respawnPoint.savePointName);
    }

    public void LoadState()
    {
        currency = PlayerPrefs.GetInt("currency", 0);
        upgrades.dash = PlayerPrefs.GetInt("dash", 0) == 1;
        upgrades.doubleJump = PlayerPrefs.GetInt("doubleJump", 0) == 1;
        respawnPoint.sceneName = PlayerPrefs.GetString("sceneName", "Level1");
        respawnPoint.savePointName = PlayerPrefs.GetString("savePointName", "");
    }

    /*
     * called when script is loaded
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // handle scene setup
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(respawnPoint.sceneName);
        // After scene loads, place player at `savePointName` location
    }


    // ===== SETTER FUNCTIONS =====
    public void unlockDash()
    {
        upgrades.dash = true;
        PlayerPrefs.SetInt("dash", 1);
    }

    public void unlockDoubleJump()
    {
        upgrades.doubleJump = true;
        PlayerPrefs.SetInt("doubleJump", 1);
    }

    public void setRespawnPoint(string savePointName = "none")
    {
        // set respawn point scene locally & persistently
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        respawnPoint.sceneName = SceneManager.GetActiveScene().name;

        // set respawn point position locally & persistently
        PlayerPrefs.SetString("savePointName", savePointName);
        respawnPoint.savePointName = savePointName;
    }

    public void addCurrency(int amt)
    {
        currency += amt;
        PlayerPrefs.SetInt("currency", currency);
    }
}
