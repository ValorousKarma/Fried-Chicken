using UnityEngine;

public class VisibleWhenNear : MonoBehaviour
{

    public float appearDistance = 6f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    // bruh why isn't this compiling
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < appearDistance)
        {
            sprite.enabled = true;
        } else
        {
            sprite.enabled = false;
        }
    }
}
