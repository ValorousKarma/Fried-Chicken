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

    public event System.Action<int> OnCurrencyChanged;
    public event System.Action<int> OnWeaponLevelChanged;
    // References
    public Player player;

    // Logic
    public int currency;
    public int weaponLevel;
    public int[] upgradeCosts = new int[2] { 50, 100 };
    public const int MAX_LEVEL = 2;

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


    // temp value for respawn logic
    private bool goToSave = false;
    protected CanvasGroup respawnCanvas;
    private bool showRespawnCanvasAfterLoad = false;

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
        PlayerPrefs.SetInt("weaponLevel", weaponLevel);
        PlayerPrefs.SetInt("dash", dash ? 1 : 0);
        PlayerPrefs.SetInt("doubleJump", doubleJump ? 1 : 0);
        PlayerPrefs.SetString("sceneName", sceneName);
        PlayerPrefs.SetString("savePointName", savePointName);
        previousSceneName = sceneName;
    }

    public void LoadState()
    {
        currency = PlayerPrefs.GetInt("currency", 0);
        weaponLevel = PlayerPrefs.GetInt("weaponLevel", 0);
        dash = PlayerPrefs.GetInt("dash", 0) == 1;
        doubleJump = PlayerPrefs.GetInt("doubleJump", 0) == 1;

        // for saving respawn point
        sceneName = PlayerPrefs.GetString("sceneName", "StartScene");
        savePointName = PlayerPrefs.GetString("savePointName", "");
    }

    /*
     * called when script is loaded
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadState();
        if (scene.name != "StartScene") 
        { 
            player = GameObject.Find("Player").GetComponent<Player>();

            if (!goToSave)
            {
                if (scene.name == "Level_Two_Town" && previousSceneName == "Level_Three_Cave")
                    player.transform.position = levelTwoSecondarySpawn;

                if (scene.name == "Level_One_Forrest" && previousSceneName == "Level_Two_Town")
                    player.transform.position = levelOneSecondarySpawn;
            }
            else
            {
                goToSave = false;
                player.transform.position = GameObject.Find(savePointName).transform.position - new Vector3(0, 2f, 0);
            }

            // Show and fade canvas AFTER scene is fully loaded
            if (showRespawnCanvasAfterLoad)
            {
                showRespawnCanvasAfterLoad = false;

                GameObject canvasObj = GameObject.Find("RespawnSetCanvas");
                if (canvasObj != null)
                {
                    respawnCanvas = canvasObj.GetComponent<CanvasGroup>();
                    if (respawnCanvas != null)
                    {
                        respawnCanvas.alpha = 1f;
                        StartCoroutine(FadeOut());
                    }
                }
            }
        }

        previousSceneName = "";
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }

    public void RespawnPlayer(bool fromSavePoint = false)
    {
        goToSave = true;
        if (fromSavePoint)
            showRespawnCanvasAfterLoad = true;
        SceneManager.LoadScene(sceneName);

        player.GetComponent<PlayerMovement>().hitpoint = player.GetComponent<PlayerMovement>().maxHitpoint;
        HealthBar.Instance.SetHealth((int)player.GetComponent<PlayerMovement>().hitpoint);
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float startAlpha = respawnCanvas.alpha;
        float elapsed = 0f;

        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;
            respawnCanvas.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / 2f);
            yield return null;
        }

        respawnCanvas.alpha = 0f;
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
        OnCurrencyChanged?.Invoke(currency);
    }

    public void deathModifyCurrency()
    {
        currency = currency / 2;
        PlayerPrefs.SetInt("currency", currency);
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool UpgradeWeapon()
    {
        if (weaponLevel < MAX_LEVEL && currency >= upgradeCosts[weaponLevel])
        {
            currency -= upgradeCosts[weaponLevel];
            weaponLevel++;
            PlayerPrefs.SetInt("weaponLevel", weaponLevel);
            OnWeaponLevelChanged?.Invoke(weaponLevel); // Trigger event
            return true;
        }
        return false;
    }

    public string GetPreviousScene()
    {
        return previousSceneName;
    }
}
