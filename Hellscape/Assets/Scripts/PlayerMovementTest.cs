using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WallDirection
{
    Left = 1,
    Right = -1
}

public class PlayerMovementTest : MonoBehaviour
{
    // Inspector Fields
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float dashForce = 20.0f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float wallJumpForce = 15.0f;
    [SerializeField] private float wallStickDuration = 0.5f;
    [SerializeField] private float gravityForce = 1.0f;
    [SerializeField] private float pogoBounceForce = 10.0f;
    [SerializeField] private float pogoBounceDuration = 0.2f;
    [SerializeField] private LayerMask ground; // The LayerMask necessary for the object to recognize if it is on the ground or not

    // Powerup booleans
    [SerializeField] public bool hasDoubleJump;
    [SerializeField] public bool hasDash;
    [SerializeField] private bool hasWallJump;
    [SerializeField] public bool hasPogoBounce;

    // Powerup facilitation booleans
    private bool canDoubleJump;
    private bool canDash;

    private Rigidbody2D rb;

    private int moveDirection; //Will only ever be -1, 0, or 1
    private int storedMoveDirection;

    // Dash Fields
    private bool isDashing;
    private float dashTimer;

    // Grounded fields
    private bool isGrounded;
    private Transform groundCheck;
    const float groundCheckRadius = 0.1f;

    // Wall Stick/Jump fields
    private bool isOnWall;
    private Transform[] wallChecks;
    const float wallCheckRadius = 0.2f;
    private WallDirection wallDirection;
    private float wallStickTimer;

    // Pogo Bounce fields
    private GameObject pogoBounceTrigger;
    private PogoBounce pogoBounceScript;

    // special mechanics
    public Vector3 spawnPoint;
    public int keys = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize necessary variables
        // Set up Ground Check
        groundCheck = transform.Find("GroundCheck");

        // Set up Wall Checks
        wallChecks = new Transform[2];
        wallChecks[0] = transform.Find("LeftWallCheck");
        wallChecks[1] = transform.Find("RightWallCheck");

        // Set up Pogo Bounce Trigger and Script
        pogoBounceTrigger = GameObject.Find("PogoBounceTrigger");
        pogoBounceScript = pogoBounceTrigger.GetComponent<PogoBounce>();

        rb = GetComponent<Rigidbody2D>();

        //Set the Gravity Scale
        rb.gravityScale = gravityForce;

        // set inital spawn point
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnWall(); // Checks to see if the player is on a wall
        CheckGrounded(); // Checks to see if the player is grounded
        GetHorizontalInput(); //Determines the move direction (-1, 0, 1)

        // Update for when the player is not in a state
        if (!isDashing && !isOnWall)
        {
            // Move either left, right, or nowhere based on the move direction
            rb.AddForce(new Vector2(acceleration * moveDirection, 0));

            // Horizontal Speed Capping
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }

            // Jump Control
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Perform a regular jump when grounded
                if (isGrounded)
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
                // Perform a double jump when not grounded
                else if (!isGrounded && canDoubleJump && hasDoubleJump)
                {
                    canDoubleJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y velocity first
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
            }

            // Dash Control
            if (Input.GetKeyDown(KeyCode.Space) && hasDash && canDash)
            {
                    // Set up horizontal dash
                    canDash = false;
                    dashTimer = dashDuration;
                    rb.AddForce(new Vector2(dashForce * storedMoveDirection, 0), ForceMode2D.Impulse);
                    isDashing = true;
            }

            // Pogo Bounce Control
            if (Input.GetKeyDown(KeyCode.LeftShift) && hasPogoBounce)
            {
                pogoBounceTrigger.SetActive(true);
                pogoBounceScript.SetTimer(pogoBounceDuration);
            }
        }

        // Update for when the player is dashing
        else if (isDashing)
        {
            // Decrease the timer and ensure that there is no y velocity every frame
            rb.velocity = new Vector2(rb.velocity.x, 0);
            dashTimer -= Time.deltaTime;

            // When the timer ends, end the dash
            if (dashTimer <= 0 || isOnWall)
            {
                isDashing = false;
            }
        }

        // Update for when the player is stuck to a wall
        else if (isOnWall) 
        {
            // If the player inputs jump, leap off of the wall
            if (Input.GetKeyDown(KeyCode.W) && hasWallJump)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(wallJumpForce * (int)wallDirection, wallJumpForce), ForceMode2D.Impulse);
            }

            // If the player is inputting a direction. . .
            if (moveDirection == (int)wallDirection)
            {
                //. . . run the wall stick timer to unstick
                wallStickTimer -= Time.deltaTime;
            }
            else
            {
                // If there is no input, reset the timer
                wallStickTimer = wallStickDuration;
            }

            // If the wall stick timer runs out, release the player with force off of the wall
            if (wallStickTimer <= 0)
            {
                rb.AddForce(new Vector2(acceleration * moveDirection, 0));
            }
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
                //. . . update isGrounded, canDoubleJump, and canDash to be true and return
                isGrounded = true;
                canDoubleJump = true;
                canDash = true;

                // If the object is on a wall, detatch it from the wall
                if (isOnWall)
                {
                    isOnWall = false;
                    rb.gravityScale = gravityForce;
                }

                return;
            }
        }

        //If there are no other colliders found, return false
        isGrounded = false;
    }

    /// <summary>
    ///  Updates isOnWall by checking objects with the "Wall" tag
    /// </summary>
    private void CheckOnWall()
    {
        // Will not perform if grounded
        if (isGrounded)
        {
            return;
        }

        //Create an array of colliders based on everything that overlaps with the wallChecks in a radius
        Collider2D[] leftColliders = Physics2D.OverlapCircleAll(wallChecks[0].position, wallCheckRadius);
        Collider2D[] rightColliders = Physics2D.OverlapCircleAll(wallChecks[1].position, wallCheckRadius);

        //Loop through the colliders found
        for (int i = 0; i < leftColliders.Length; i++)
        {
            //If there is an object in the array (excluding this object). . .
            if (leftColliders[i].gameObject.tag == "Wall")
            {
                //. . . update isOnWall to be true and return
                if (isOnWall == false)
                {
                    // Also change the gravity and update a few variables
                    rb.gravityScale = 0.5f;
                    wallStickTimer = wallStickDuration;
                    wallDirection = WallDirection.Left;
                }
                isOnWall = true;
                return;
            }
        }
        for (int i = 0; i < rightColliders.Length; i++)
        {
            //If there is an object in the array (excluding this object). . .
            if (rightColliders[i].gameObject.tag == "Wall")
            {
                //. . . update isOnWall to be true and return
                if (isOnWall == false)
                {
                    // Also change the gravity and update a few variables
                    rb.gravityScale = 0.5f;
                    wallStickTimer = wallStickDuration;
                    wallDirection = WallDirection.Right;
                }
                isOnWall = true;
                return;
            }
        }

        //If there are no other colliders found, return false
        if (isOnWall == true)
        {
            rb.gravityScale = gravityForce;
        }
        isOnWall = false;
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

        if (moveDirection != 0)
        {
            storedMoveDirection = moveDirection;
        }
    }

    /// <summary>
    ///  Bounces the player object upwards when called
    /// </summary>
    public void PogoBounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, pogoBounceForce), ForceMode2D.Impulse);
    }

    public void Die()
    {
        transform.position = spawnPoint;
        rb.velocity = Vector2.zero;
    }
}
