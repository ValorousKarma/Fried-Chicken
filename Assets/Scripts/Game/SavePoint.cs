using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Interact") && Vector3.Distance(Player.Instance.transform.position, transform.position) < 3)
        {
            GameState.Instance.SetRespawnPoint(gameObject.name);
            GameState.Instance.SaveState();
            GameState.Instance.RespawnPlayer(true);
        }
    }
}
