using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Player").GetComponent<PlayerMovementTest>().Die();
    }
}
