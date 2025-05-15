using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlOneTutorialScript : MonoBehaviour
{
    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        if(GameState.Instance.GetNewGame())
        {
            popup.SetActive(true);
            GameState.Instance.SetNewGame(false);
        }
    }
}
