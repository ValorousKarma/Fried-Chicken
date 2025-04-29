using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;

    public float boundX = 0.3f;

    private void LateUpdate()
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

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
