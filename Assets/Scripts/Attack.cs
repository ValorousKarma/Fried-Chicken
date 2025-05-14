using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Collidable
{
    private Animator anim;
    public PolygonCollider2D attackHitbox;
    public float ATTACK_LENGTH = 0.267f;
    private float lastHit = 0f;
    public string excludeNameFromDamage;

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
        // inherit collision iteration from parent
        base.Update();

        if (Time.time - lastHit > ATTACK_LENGTH)
        {
            attackHitbox.enabled = false;
        }
    }

    public void PerformAttack()
    {
        if (Time.time - lastHit > attackCooldown)
        {
            attackHitbox.enabled = true;
            lastHit = Time.time;
            anim.SetTrigger("attack");

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPlayerAttackSound();
        }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (coll.tag == "Damageable")
        {
            if (coll.name != excludeNameFromDamage)
            {
                Damage dmg = new Damage
                {
                    dmg = attackDamage,
                    origin = transform.position,
                    pushForce = pushForce,
                };

                coll.SendMessage("ReceiveDamage", dmg);
            }
        } else if (coll.tag == "Player")
        {
            if (coll.name != excludeNameFromDamage)
            {
                Damage dmg = new Damage
                {
                    dmg = attackDamage,
                    origin = transform.position,
                    pushForce = pushForce,
                };

                coll.GetComponentInParent<Attackable>().SendMessage("ReceiveDamage", dmg);
            }
        }
    }
}
