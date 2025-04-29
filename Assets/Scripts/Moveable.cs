using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Moveable : Attackable
{
    [SerializeField] public const float MOVE_SPEED = 1.0F;
    protected const int pixelsPerUnit = 16;
    protected Rigidbody2D rb;
    protected Vector2 moveDelta;
    protected RaycastHit2D hit;

    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void UpdateMotor(Vector2 inp)
    {

        // Reset moveDelta
        moveDelta = new Vector2(inp.x * xSpeed, inp.y * ySpeed);
        Vector2 targetPosition = rb.position + moveDelta * Time.fixedDeltaTime;

        // Swap sprite direction
        if (moveDelta.x > 0)
            transform.localScale = new Vector2(1, 1);
        else if (moveDelta.x < 0)
            transform.localScale = new Vector2(-1, 1);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, based off recovery speed
        pushDirection = Vector2.Lerp(pushDirection, Vector2.zero, pushRecoverySpeed);

        Vector2 moveAmount = moveDelta * MOVE_SPEED * Time.fixedDeltaTime;

        // Wall detection: check if the movement direction collides with a wall and avoid sticking
        if (moveDelta.x != 0) // Horizontal movement
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(moveDelta.x, 0), Mathf.Abs(moveAmount.x), LayerMask.GetMask("Blocking"));
            if (hit.collider != null)
            {
                // If a wall is detected, set horizontal movement to zero
                moveAmount.x = 0;
            }
        }

        if (moveDelta.y != 0) // Vertical movement
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(0, moveDelta.y), Mathf.Abs(moveAmount.y), LayerMask.GetMask("Blocking"));
            if (hit.collider != null)
            {
                // If a wall is detected, set vertical movement to zero
                moveAmount.y = 0;
            }
        }


        rb.MovePosition(rb.position + moveAmount);
    }
}
