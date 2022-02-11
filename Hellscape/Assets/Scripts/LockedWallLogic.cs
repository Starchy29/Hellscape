using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedWallLogic : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerMovementTest player = GameObject.Find("Player").GetComponent<PlayerMovementTest>();
        if(player.keys > 0) {
            player.keys--;
            Destroy(this.gameObject);
        }
    }
}
