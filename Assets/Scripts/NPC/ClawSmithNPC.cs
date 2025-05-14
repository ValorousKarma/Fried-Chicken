using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClawSmithNPC : StaticNPC
{
    private int dialogueZeroChoice = 4;
    private int dialogueOneChoice = 2;
    protected Button yesOption;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dialogueSeries = new string[][]
        {
            new string[] { "Man... I'm hungry...", "Can't believe I got stuck in this cave.", "Oh, you look like a nice racoon!", "Bring me some eggs to cook up and I'll sharpen yer claws!", "Give the man " + GameState.Instance.upgradeCosts[0] + " eggs?" },
            new string[] { "Those eggs were TAAAAASTY.", "I'll tell you what, bring me some more eggs and I'll make those claws DEADLY.", "Give the man " + GameState.Instance.upgradeCosts[1] + " eggs?" },
            new string[] { "I'm all full now." }
        };

        yesOption = options.transform.GetChild(0).GetComponent<Button>();
    }

    // Update is called once per frame
    protected override void Update()
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

                    // handle displaying player choices on appropriate dialogue lines
                    if (currentDialogue == 0)
                    {
                        if (currentLine == dialogueZeroChoice)
                        {
                            showOptions();
                        }
                    } else if (currentDialogue == 1)
                    {
                        if (currentLine == dialogueOneChoice)
                        {
                            showOptions();
                        }
                    }
                }
                else
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
                startDialogue(GameState.Instance.weaponLevel);
                verifyCanUpgrade();
            }
        }
    }

    // upgrade those claws
    public void UpgradeClaws()
    {
        exitDialogue();
        GameState.Instance.UpgradeWeapon();
    }

    protected void verifyCanUpgrade()
    {
        // avoid index out of bounds
        if (GameState.Instance.weaponLevel < GameState.Instance.upgradeCosts.Length)
        {
            // only allow player to select upgrade option if they have the moneeeeeey
            if (GameState.Instance.currency >= GameState.Instance.upgradeCosts[GameState.Instance.weaponLevel])
            {
                yesOption.interactable = true;
            }
            else
            {
                yesOption.interactable = false;
            }
        }
    }
}
