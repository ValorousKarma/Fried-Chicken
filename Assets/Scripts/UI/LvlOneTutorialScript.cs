using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlOneTutorialScript : MonoBehaviour
{
    public GameObject popup;
    public GameState state;
    // Start is called before the first frame update
    void Start()
    {
        state = FindObjectOfType<GameState>();
        //if (state.savePointName == "" && state.previousScene == "") 
        if(PlayerPrefs.GetString("savePointName", "") == "")
        {
            popup.SetActive(true);
        }
    }
}
