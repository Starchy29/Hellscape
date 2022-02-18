using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player") {
            GameObject.Find("Player").GetComponent<PlayerMovementTest>().keys++;
            this.gameObject.SetActive(false);
        }
    }
}
