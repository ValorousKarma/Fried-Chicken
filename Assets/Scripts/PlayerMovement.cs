using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Misc")]
    protected Animator anim;
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 40f;
    public float airControl = 0.5f;


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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle horizontal input
        moveInput = Input.GetAxisRaw("Horizontal");

        // face direction of input
        if (moveInput != 0)
            transform.localScale = new Vector2(moveInput * Mathf.Abs(transform.localScale.x), 1 * transform.localScale.y);

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

        HandleAnimator();
    }

    private void FixedUpdate()
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
}
