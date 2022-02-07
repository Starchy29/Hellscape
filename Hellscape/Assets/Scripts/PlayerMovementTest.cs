using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private LayerMask ground; // The LayerMask necessary for the object to recognize if it is on the ground or not

    private Rigidbody2D rb;

    private int moveDirection; //Will only ever be -1, 0, or 1

    private bool isGrounded;
    private Transform groundCheck;
    const float groundCheckRadius = 0.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize necessary variables
        groundCheck = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded(); //Checks to see if the player is grounded
        GetHorizontalInput(); //Determines the move direction (-1, 0, 1)

        // Move either left, right, or nowhere based on the move direction
        rb.AddForce(new Vector2(acceleration * moveDirection, 0));

        // Horizontal Speed Capping
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed * moveDirection, rb.velocity.y);
        }

        // Jump Control
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    /// <summary>
    ///  Updates isGrounded using ground to determine if the groundCheck is touching the ground
    /// </summary>
    private void CheckGrounded()
    {
        //Create an array of colliders based on everything that overlaps with the groundCheck in a radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, ground);

        //Loop through the colliders found
        for (int i = 0; i < colliders.Length; i++)
        {
            //If there is an object in the array (excluding this object). . .
            if (colliders[i].gameObject != gameObject)
            {
                //. . . update isGrounded to be true and return
                isGrounded = true;
                print(isGrounded);
                return;
            }
        }

        //If there are no other colliders found, return false
        isGrounded = false;
        print(isGrounded);
    }

    /// <summary>
    ///  Updates moveDirection to be -1, 0, or 1 based on the direction the player is inputting with A and D
    /// </summary>
    private void GetHorizontalInput()
    {
        moveDirection = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += 1;
        }
    }
}
