using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Physics2DModule;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float acceleration = 20f;
    public float jumpSpeed = 5;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // move left / right
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            body.AddForce(new Vector2(acceleration, 0));
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            body.AddForce(new Vector2(-acceleration, 0));
        }

        // cap horizontal speed
        if(body.velocity.x > maxSpeed) {
            body.velocity = new Vector2(maxSpeed, body.velocity.y);
        }
        else if(body.velocity.x < -maxSpeed) {
            body.velocity = new Vector2(-maxSpeed, body.velocity.y);
        }

        // jump
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }
}
