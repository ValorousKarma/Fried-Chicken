using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Attackable : MonoBehaviour
{

    [Header("Receiving Damage")]
    public float hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoverySpeed = 0.2f;
    public float immuneTime = 1.0f;

    [Header("Damage Animations")]
    public Animator anim;

    [Header("Physics")]
    public float knockbackMultiplier = 5f;

    protected float lastImmune;
    protected bool invincible = false;

    protected Vector2 pushDirection;

    protected virtual void Update()
    {
        // handle invincibility state
        if (Time.time - lastImmune <= immuneTime)
        {
            // do nothing
        } else if (invincible && (Time.time - lastImmune > immuneTime)) {
            invincible = false;
            if (anim != null)
            {
                anim.SetBool("Invincible", false);
            }
        }
    }

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.dmg;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce * knockbackMultiplier;

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            } else
            {
                if (!invincible)
                    invincible = true;
                if (anim != null)
                {
                    anim.SetBool("Invincible", true);
                }
            }
        }
    }

    protected virtual void Death()
    {

    }
}
