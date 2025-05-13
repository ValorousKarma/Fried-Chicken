using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpgrade : MonoBehaviour
{
    private Animator anim;
    private bool collected;

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
