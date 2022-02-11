using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingWallScript : MonoBehaviour
{
    private float timer = 0f;

    private void Update()
    {
        if(timer > 0) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                timer = -5f; // respawn time
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if(timer < 0) {
            timer += Time.deltaTime;
            if(timer >= 0) {
                timer = 0;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        timer = 2f; // time after collision before destruction
    }
}
