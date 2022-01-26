using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Physics2DModule;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 5f;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(body.velocity.x <= maxSpeed && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) {
            //transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
            body.AddForce(new Vector2(50, 0));
        }
        //if()
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //transform.position = transform.position + new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }
}
