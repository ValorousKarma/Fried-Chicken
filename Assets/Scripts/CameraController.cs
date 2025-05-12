using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;

    public float heightOffset;

    public float boundX = 0.3f;
    public float boundY = 0.3f;

    private void Update()
    {
        Vector3 delta = Vector3.zero;

        // check if inside x axis bounds
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            } else
            {
                delta.x = deltaX + boundX;
            }
        }

        // check if inside y axis bounds
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            } else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y + heightOffset, 0);
    }
}
