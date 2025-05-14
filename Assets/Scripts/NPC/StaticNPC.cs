using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class StaticNPC : MonoBehaviour
{
    protected string[][] dialogueSeries;
    protected int currentLine;
    protected int currentDialogue;

    private GameObject npc;
    protected GameObject dialogueBox;
    protected GameObject options;
    private TextMeshProUGUI text;

    protected virtual void Start()
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
        dialogueSeries = new string[][] { };
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        // handle player input if dialogue is active
        if (dialogueBox.activeSelf)
        {
            if (Input.GetButtonDown("Interact"))
            {
                // prevent ArrayOutOfBounds
                if (currentLine + 1 < dialogueSeries[currentDialogue].Length)
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

    protected virtual void startDialogue(int dialogueSelection = 0, int line = 0)
    {
        // show dialogue box
        dialogueBox.SetActive(true);
        options.SetActive(false);
        text.text = dialogueSeries[dialogueSelection][line];
        currentDialogue = dialogueSelection;
        currentLine = line;

    }

    protected virtual void nextDialogue()
    {
        // increment dialogue index by one and display new dialogue
        ++currentLine;
        text.text = dialogueSeries[currentDialogue][currentLine];

    }

    public virtual void exitDialogue()
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
    protected virtual void showRandomLine(int dialogueSelection = 0)
    {

        while (true) {
            int nextLine = Random.Range(0, dialogueSeries[dialogueSelection].Length);
            
            // change displayed dialogue to new random line
            if (nextLine == currentLine)
            {
                // make sure not to infinitely loop if not enough dialogue options
                if (dialogueSeries[dialogueSelection].Length < 2)
                {
                    break;
                }
                // try again, generate another random dialogue option
            } else
            {
                currentLine = nextLine;
                text.text = dialogueSeries[dialogueSelection][currentLine];
                break;
            }
        }
    }
}
