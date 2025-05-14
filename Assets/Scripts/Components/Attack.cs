using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool isPlayer = false;
    public int[] attackDamage = new int[3];
    public float[] pushForce = new float[3];

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
        if (!isPlayer)
        {
            if (coll.tag == "Damageable")
            {
                if (coll.name != excludeNameFromDamage)
                {
                    Damage dmg = new Damage
                    {
                        dmg = attackDamage[0],
                        origin = transform.position,
                        pushForce = pushForce[0],
                    };

                    coll.SendMessage("ReceiveDamage", dmg);
                }
            }
            else if (coll.tag == "Player")
            {
                if (coll.name != excludeNameFromDamage)
                {
                    Damage dmg = new Damage
                    {
                        dmg = attackDamage[0],
                        origin = transform.position,
                        pushForce = pushForce[0],
                    };

                    coll.GetComponentInParent<Attackable>().SendMessage("ReceiveDamage", dmg);
                }
            }
        } else
        {
            if (coll.tag == "Damageable")
            {
                if (coll.name != excludeNameFromDamage)
                {
                    Damage dmg = new Damage
                    {
                        dmg = attackDamage[GameState.Instance.weaponLevel],
                        origin = transform.position,
                        pushForce = pushForce[GameState.Instance.weaponLevel],
                    };

                    coll.SendMessage("ReceiveDamage", dmg);
                }
            }
            else if (coll.tag == "Player")
            {
                if (coll.name != excludeNameFromDamage)
                {
                    Damage dmg = new Damage
                    {
                        dmg = attackDamage[GameState.Instance.weaponLevel],
                        origin = transform.position,
                        pushForce = pushForce[GameState.Instance.weaponLevel],
                    };

                    coll.GetComponentInParent<Attackable>().SendMessage("ReceiveDamage", dmg);
                }
            }
        }
    }
}
