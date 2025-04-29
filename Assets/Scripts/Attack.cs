using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Collidable
{
    private Animator anim;
    private PolygonCollider2D attackHitbox;
    private float ATTACK_LENGTH = 0.267f;
    private float lastHit = 0f;

    [Header("Timing")]
    public float attackCooldown = 0.5f;

    [Header("Damage")]
    public int attackDamage = 1;
    public float pushForce = 2.0f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        attackHitbox = transform.GetComponentInChildren<PolygonCollider2D>();
        attackHitbox.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // inherit collision itteration from parent
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastHit > attackCooldown)
            {
                attackHitbox.enabled = true;
                lastHit = Time.time;
                anim.SetTrigger("attack");
            }
        } else {
            if (Time.time - lastHit > ATTACK_LENGTH)
            {
                attackHitbox.enabled = false;
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (coll.tag == "Damageable")
        {
            if (coll.name != "Player")
            {
                Damage dmg = new Damage
                {
                    dmg = attackDamage,
                    origin = transform.position,
                    pushForce = pushForce,
                };

                coll.SendMessage("ReceiveDamage", dmg);
            }
        }
    }
}
