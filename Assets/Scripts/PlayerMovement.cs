using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Attackable
{
    
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 40f;
    public float airControl = 0.5f;
    public float dashSpeed = 40f;
    public float dashCooldown = 0.5f;


    [Header("Jumping")]
    public float jumpForce = 16f;
    public bool variableJump = true;
    public float jumpCutMultiplier = 0.5f;

    [Header("Jump Buffering")]
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isJumping;
    
    private bool isDashing;
    private Vector2 facing;
    private float lastDash = 0;
    private float dashDuration = 0.25f;

    private float attackBufferCounter = 0f;
    private float attackBufferTime = 0.1f;

    protected override void Start()
    {
        base.Start();
        facing = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
        // Handle horizontal input
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            attackBufferCounter = attackBufferTime;
        } else if (attackBufferCounter > 0f)
        {
            attackBufferCounter -= Time.deltaTime;
        }

        if (attackBufferCounter > 0 && !isDashing)
        {
            attackBufferCounter = 0f;
            GetComponent<Attack>().PerformAttack();
        }

        // face direction of input
        if (moveInput != 0)
        {
            transform.localScale = new Vector2(moveInput * Mathf.Abs(transform.localScale.x), 1 * transform.localScale.y);
            facing = transform.localScale;
        }

        // Jump buffer: store time since jump was pressed
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // cut jump when button released early
        if (variableJump && Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
            }
            isJumping = false;
        }

        if (Input.GetButtonDown("Dash") && ((Time.fixedTime - lastDash) > dashCooldown))
        {
            GetComponentInChildren<Attack>().attackHitbox.enabled = false;
            lastDash = Time.fixedTime;
            StartCoroutine(PerformDash());
        }

        HandleAnimator();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            isGrounded = IsGrounded();

            if (isGrounded && jumpBufferCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpBufferCounter = 0f;  // Reset the buffer
            }

            float targetSpeed = moveInput * moveSpeed;
            float speedDiff = targetSpeed - rb.velocity.x;

            float accelRate;

            // choose acceleration rate if there is movement input
            if (Mathf.Abs(targetSpeed) > 0.01f)
            {
                if (isGrounded)
                {
                    accelRate = acceleration;
                }
                else
                {
                    accelRate = acceleration * airControl;
                }
            }
            // otherwise, slow to a stop
            else
            {
                if (isGrounded)
                {
                    accelRate = deceleration;
                }
                else
                {
                    accelRate = deceleration * airControl;
                }
            }

            float desiredVelocityX = moveInput * moveSpeed;
            float smoothedX = Mathf.MoveTowards(rb.velocity.x, desiredVelocityX, accelRate * Time.fixedDeltaTime);
            rb.velocity = new Vector2(smoothedX, rb.velocity.y);

            // incorporate knockback from attacks into current velocity
            if (pushDirection.magnitude > 0.1f)
            {
                rb.velocity += pushDirection;

                pushDirection = Vector2.Lerp(pushDirection, Vector2.zero, pushRecoverySpeed);
            }
            else
                pushDirection = Vector2.zero;
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    /*
     * Update animator parameters with player state in order to trigger correct animations
     */
    protected void HandleAnimator()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isJumping", isJumping);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(facing.normalized.x * dashSpeed, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
}
