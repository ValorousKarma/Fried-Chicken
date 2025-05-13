using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private Animator anim;
    private bool collected;
    public string toScene;

    private void Start()
    {
        anim = GetComponent<Animator>();
        collected = false;
    }
    void Update()
    {
        if (!collected && Input.GetButtonDown("Interact") && Vector3.Distance(Player.Instance.transform.position, transform.position) < 1.5)
        {
            GameState.Instance.SaveState();
            SceneManager.LoadScene(toScene);
        }
    }
}
