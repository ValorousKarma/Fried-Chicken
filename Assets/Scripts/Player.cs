using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

}
