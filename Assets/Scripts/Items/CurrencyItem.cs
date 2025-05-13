using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyItem : CollectibleItem
{
     public int amount = 10;

    public override void collect()
    {
        base.collect();
        GameState.Instance.AddCurrency(amount);
        Debug.Log($"Player collected {amount} currency!");
    }
}
