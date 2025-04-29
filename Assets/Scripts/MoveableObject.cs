using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : Moveable
{
    private void FixedUpdate()
    {
        UpdateMotor(Vector2.zero);
    }
}
