using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 40f;
    public float airControl = 0.5f;


    [Header("Jumping")]
    public float jumpForce = 16f;
    public bool variableJump = true;
    public float jumpCutMultiplier = 0.5f;


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
    }

    void Update()
    {
        // Handle horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");
        
        // handle jump input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
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
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

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

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
        rb.AddForce(Vector2.right * movement);
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
}
