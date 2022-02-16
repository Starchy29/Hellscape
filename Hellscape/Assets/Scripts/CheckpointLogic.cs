using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player") {
            GameObject.Find("Player").GetComponent<PlayerMovementTest>().spawnPoint = this.transform.position;
        }
    }
}
