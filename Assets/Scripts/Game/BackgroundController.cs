using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Vector3 startPos, length;
    public GameObject cam;
    public float parallaxEffect; // relative speed background should move

    private void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;

    }

    private void LateUpdate()
    {
        // calculate distance background should move
        // 0 = move exactly with camera <=> 1 = don't move at all
        Vector3 distance = cam.transform.position * parallaxEffect;
        Vector3 movement = cam.transform.position * (1 - parallaxEffect);
        

        transform.position = new Vector3(startPos.x + distance.x, transform.position.y, transform.position.z);

        // if background reached end of length adjust its position for infinite scrolling
        if (movement.x > startPos.x + length.x)
        {
            startPos.x += length.x;
        } else if (movement.x < startPos.x - length.x)
        {
            startPos.x -= length.x;
        }
    }
}
