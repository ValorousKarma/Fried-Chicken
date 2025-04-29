using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator anim;
    private PolygonCollider2D attackHitbox;
    private float ATTACK_LENGTH = 0.267f;
    public float attackCooldown = 0.5f;
    private float lastHit = 0f;
    private bool attacking = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        attackHitbox = transform.GetComponentInChildren<PolygonCollider2D>();
        attackHitbox.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastHit > attackCooldown)
            {
                attackHitbox.enabled = true;
                lastHit = Time.time;
                anim.SetTrigger("attack");
                attacking = true;
            }
        } else {
            if (Time.time - lastHit > ATTACK_LENGTH)
            {
                attacking = false;
                attackHitbox.enabled = false;
            }
        }
    }
}
