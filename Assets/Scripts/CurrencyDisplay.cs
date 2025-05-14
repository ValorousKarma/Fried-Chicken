using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText; // For standard UI Text
    // public TextMeshProUGUI currencyText; // For TextMeshPro

    private void Start()
    {
        // Initialize with current currency
        UpdateDisplay(GameState.Instance.currency);
        
        // Subscribe to currency changes
        GameState.Instance.OnCurrencyChanged += UpdateDisplay;
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        if (GameState.Instance != null)
        {
            GameState.Instance.OnCurrencyChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(int amount)
    {
        if (currencyText != null)
        {
            currencyText.text = "x "+amount.ToString();
        }
    }
}
