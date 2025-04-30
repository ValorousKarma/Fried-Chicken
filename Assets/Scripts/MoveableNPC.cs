using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class MoveableNPC : Moveable
{
    [Header("Rewards")]
    public int rewards = 5;

    [Header("Idle behavior")]
    public float idleSpeed = 2.5f;

    [Header("Chasing behavior")]
    public float triggerDistance = 5f;
    public float chaseDistance = 16f;
    public float chasingSpeed = 6f;

    [Header("Attacking behavior")]
    public float playerAttackRange = 0.1f;
    public float attackCooldown = 1f;
    protected float lastHit = 0f;

    // struct to store enemy state info
    // override in child class if necessary
    protected struct State
    {
        public bool chasing;
        public bool playerInRange;
        public bool attacking;
        public bool idleing;
    }

    protected State state;

    protected Transform playerTransform;
    protected Vector3 startingPosition;
    public ContactFilter2D filter;

    protected override void Start()
    {
        base.Start();

        playerTransform = Player.Instance.transform;
        startingPosition = transform.position;

        // set default states
        state.chasing = false;
        state.playerInRange = false;
        state.attacking = false;
        state.idleing = true;
    }

    /*
     * Handle basic enemy behavior states
     */
    protected override void Update()
    {
        base.Update();
        float playerDistance = Vector3.Distance(playerTransform.position, this.transform.position);

        // if not chasing and player gets too close, chase
        if (playerDistance < triggerDistance && !state.chasing)
        {
            state.chasing = true;
            state.idleing = false;
            xSpeed = chasingSpeed;
        // if chasing and player gets far away enough, stop chasing and return to idle behavior
        } else if (playerDistance > chaseDistance && state.chasing)
        {
            state.chasing = false; 
        }

        // if not attacking and close enough to player to attack, attack
        if (playerDistance < playerAttackRange && !state.attacking)
        {
            state.attacking = true;
        }
        // if attacking and too far from player to attack, stop attacking
        else if (playerDistance > playerAttackRange && state.attacking)
        {
            state.attacking = false;
        }
    }
    /*
     * Define what happens when an NPC dies
     */
    protected override void Death()
    {
        Destroy(gameObject);
        // give player rewards here
    }

    /*
     * override in child class to define behavior to return from chasing to idle position
     */
    protected virtual void ReturnToStart()
    {
    }
}
