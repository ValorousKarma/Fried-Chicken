using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenNPC : MoveableNPC
{
    [Header("Rat Idle behavior")]
    public float idleRange = 3f;
    private float idleEndpoint;
    private bool idleWalkLeft;

    protected Attack attack;
    private bool isAttacking = false;
    public float startAttackDelay = 0.5f;
    public BossDefeatedPopUp bossPopup;
    protected override void Start()
    {
        base.Start();
        ySpeed = 0;
        xSpeed = idleSpeed;
        idleEndpoint = startingPosition.x - idleRange;
        idleWalkLeft = true;
        attack = GetComponent<Attack>();
    }

    protected override void Update()
    {
        base.Update();

        if (state.attacking && !isAttacking && Time.time - lastHit >= attackCooldown)
        {
            isAttacking = true;
            anim.SetTrigger("attack");
        }
    }

    protected void FixedUpdate()
    {
        Vector2 moveDir = Vector2.zero;

        /*
         * Pace back and forth until player
         * provokes rat to chase
         */
        if (state.idleing)
        {
            // handle walking left & turning right
            if (idleWalkLeft)
            {
                moveDir = new Vector2(-1, 0);
                if (Mathf.Abs(transform.position.x - idleEndpoint) < 0.1)
                    idleWalkLeft = false;
            } 
            // handle walking right * turning left
            else
            {
                moveDir = new Vector2(1, 0);
                if (Mathf.Abs(startingPosition.x - transform.position.x) < 0.1)
                    idleWalkLeft = true;
            }
        }

        /*
         * When player approaches, chase until player
         * is too far or rat is close enough to attack
         */
        if (state.chasing)
        {
            moveDir = (playerTransform.position - this.transform.position).normalized;
        }

        /*
         * When attacking the player don't keep chasing
         */
        if (state.attacking)
        {
            moveDir = Vector2.zero;
        }

        UpdateMotor(moveDir);

        // while not chasing or idleing, try to return to starting position
        if (!state.chasing && !state.idleing)
        {
            ReturnToStart();
            // if start position is reached, return to idle state
            if (Mathf.Abs(this.transform.position.x - startingPosition.x) < 0.1)
            {
                state.idleing = true;
                xSpeed = idleSpeed;
            }
        }
    }

    protected override void ReturnToStart()
    {
        Vector2 moveDir = (startingPosition - transform.position).normalized;
        UpdateMotor(moveDir);
    }

    protected override void Death()
    {
        base.Death();
        GameState.Instance.AddCurrency(rewards);
        bossPopup.gameObject.SetActive(true);
    }

    public void OnAttackHit()
    {
        attack.PerformAttack();
        AudioManager.instance.PlayChickenAttackSound();
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        lastHit = Time.time;
    }
}
