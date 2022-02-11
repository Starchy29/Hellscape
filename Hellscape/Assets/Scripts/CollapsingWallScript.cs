using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingWallScript : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private float crumbleTime = 2f;

    private void Update()
    {
        if(timer > 0) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                timer = -respawnTime;
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
        timer = crumbleTime;
    }
}
