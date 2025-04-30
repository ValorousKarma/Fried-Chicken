using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Moveable : Attackable
{
    [SerializeField] private LayerMask groundLayer;
    protected const int pixelsPerUnit = 16;
    protected Rigidbody2D rb;
    protected Vector2 moveDelta;
    protected RaycastHit2D hit;
    protected BoxCollider2D hitbox;

    protected float ySpeed = 0f;
    protected float xSpeed = 5.0f;

    protected float verticalVelocity = 0f;

    [Header("Physics")]
    public float gravityMultiplier = 3f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector2 inp)
    {
        // Horizontal movement
        moveDelta = new Vector2(inp.x * xSpeed, 0f);

        // Add gravity as acceleration
        if (!IsGrounded())
        {
            verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            verticalVelocity = 0f; // Reset vertical velocity when grounded
                                   // Horizontal movement
            moveDelta = new Vector2(inp.x * xSpeed, 0f);
        }

        // Flip sprite
        if (moveDelta.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDelta.x);
            transform.localScale = scale;
        }

        // Apply vertical velocity
        moveDelta.y = inp.y * ySpeed + verticalVelocity;

        // Add push vector
        int direction = 1;
        if (pushDirection.x < 0)
            direction = -1;

        moveDelta += new Vector2(direction * pushDirection.magnitude, 0);
        pushDirection = Vector2.Lerp(pushDirection, Vector2.zero, pushRecoverySpeed);

        // Final movement
        Vector2 moveAmount = moveDelta * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveAmount);
    }

    protected virtual bool IsGrounded()
    {
        Bounds bounds = hitbox.bounds;
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - 0.05f);
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, 0.05f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
    }
}
