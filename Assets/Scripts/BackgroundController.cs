using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect; // relative speed background should move

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        // calculate distance background should move
        // 0 = move exactly with camera <=> 1 = don't move at all
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // if background reached end of length adjust its position for infinite scrolling
        if (movement > startPos + length)
        {
            startPos += length;
        } else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
