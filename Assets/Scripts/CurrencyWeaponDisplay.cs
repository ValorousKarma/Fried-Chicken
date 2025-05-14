using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyWeaponDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText; 
    public TextMeshProUGUI weaponLevelText;

    public Image weaponLevelImage;  // Your weapon level graphic
    public Color[] levelColors;

    private void Start()
    {
        // Initialize displays
        UpdateCurrencyDisplay(GameState.Instance.currency);
        UpdateWeaponLevelDisplay(GameState.Instance.weaponLevel);
        
        // Subscribe to events
        GameState.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
        GameState.Instance.OnWeaponLevelChanged += UpdateWeaponLevelDisplay;

        weaponLevelImage.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
            GameState.Instance.OnWeaponLevelChanged -= UpdateWeaponLevelDisplay;
        }
    }

    private void UpdateCurrencyDisplay(int amount)
    {
        if (currencyText != null)
        {
            currencyText.text = "x" + amount.ToString();
        }
    }

    private void UpdateWeaponLevelDisplay(int level)
    {
        if (weaponLevelText != null)
        {
            weaponLevelText.text = "Weapon Level: "+(level + 1).ToString(); // +1 for 1-based display
        }
        
        // Update image color
        if (weaponLevelImage != null && levelColors.Length > level)
        {
            weaponLevelImage.color = levelColors[level];
        }
    }
}
