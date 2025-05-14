using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class StaticNPC : MonoBehaviour
{
    public string[] dialogueSeries;
    private int currentDialogue;

    private GameObject npc;
    private GameObject dialogueBox;
    private GameObject options;
    private TextMeshProUGUI text;

    private void Start()
    {
        // game object references for this dialogue NPC
        npc = this.gameObject;

        // expects a canvas child object
        dialogueBox = npc.transform.GetChild(0).gameObject;

        /* 
         * expects child (0) 'Options' GameObject, and child (1) 'Text' GameObject:
         * 
         * NPC (<- this script is attached here)
         *  ->Dialogue
         *      ->DialoguePanel
         *          ->Options
         *          ->Text
         */
        
        options = dialogueBox.transform.GetChild(0).GetChild(0).gameObject;
        text = dialogueBox.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        // no dialogue should be shown by default
        dialogueBox.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        // handle player input if dialogue is active
        if (dialogueBox.activeSelf)
        {
            if (Input.GetButtonDown("Interact"))
            {
                // prevent ArrayOutOfBounds
                if (currentDialogue + 1 < dialogueSeries.Length)
                {
                    nextDialogue();
                } else
                {
                    exitDialogue();
                }
            }

            // exit dialogue if player walks away from NPC
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) > 3)
            {
                exitDialogue();
            }
        } 
        // handle player input if dialogue is not currently active
        else
        {
            if (Input.GetButtonDown("Interact") && Vector3.Distance(Player.Instance.transform.position, transform.position) < 2)
            {
                startDialogue();
            }
        }
    }

    protected virtual void startDialogue(int line = 0)
    {
        // show dialogue box
        dialogueBox.SetActive(true);
        options.SetActive(false);
        text.text = dialogueSeries[line];
        currentDialogue = line;

    }

    protected virtual void nextDialogue()
    {
        // increment dialogue index by one and display new dialogue
        ++currentDialogue;
        text.text = dialogueSeries[currentDialogue];

    }

    protected virtual void exitDialogue()
    {
        dialogueBox.SetActive(false);
    }

    // show player input options
    protected virtual void showOptions()
    {
        options.SetActive(true);
    }

    // hide player input options
    protected virtual void hideOptions()
    {
        options.SetActive(false);
    }

    // display a random line of dialogue
    protected virtual void showRandomLine()
    {

        while (true) {
            int nextDialogue = Random.Range(0, dialogueSeries.Length);
            
            // change displayed dialogue to new random line
            if (nextDialogue == currentDialogue)
            {
                // make sure not to infinitely loop if not enough dialogue options
                if (dialogueSeries.Length < 2)
                {
                    break;
                }
                // try again, generate another random dialogue option
            } else
            {
                currentDialogue = nextDialogue;
                text.text = dialogueSeries[currentDialogue];
                break;
            }
        }
    }
}
