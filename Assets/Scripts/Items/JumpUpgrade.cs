using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpUpgrade : MonoBehaviour
{
    private Animator anim;
    private bool collected;

    public GameObject popup;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
        collected = false;
    }
    void Update()
    {
        if (!collected && Input.GetButtonDown("Interact") && Vector3.Distance(Player.Instance.transform.position, transform.position) < 1.5)
        {
            anim.SetTrigger("collected");
            popup.SetActive(true);
        }

        if (GameState.Instance.doubleJump)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void OnCollectionComplete()
    {
        GameState.Instance.UnlockDoubleJump();
        GameObject.Destroy(gameObject);
    }
}
