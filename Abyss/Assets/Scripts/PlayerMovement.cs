using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;  // Adjust this to set the player's movement speed
    public float jumpForce = 20f; // Adjust this to set the player's jump force
    public float jumpHoldDuration = 0.5f;
    public float dashSpeed = 30f; // Adjust this to set the dash speed
    public float dashDuration = 1f; // Adjust this to set the dash duration
    public float dashCooldown = 0.5f; // Adjust this to set the dash cooldown

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private float originalMoveSpeed;
    private bool canDash = true;
    private Transform groundCheck;
    private LayerMask groundLayer;

    private float lastGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck"); // Create an empty GameObject called "GroundCheck" slightly below the player
        groundLayer = LayerMask.GetMask("Ground"); // Make sure your ground objects are on the "Ground" layer
        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Player input for movement
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(moveInput, 0f);

        // Apply dash if not currently dashing
        if (canDash && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Dash());
        }

        // If dashing, move with dash speed
        if (isDashing)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(move.x * dashSpeed, rb.velocity.y);
        }
        else
        {
            // Otherwise, move with regular speed
            rb.gravityScale = 4;
            rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
        }


        if (isGrounded) lastGrounded = Time.time;

        // Player input for jumping

        if (Input.GetButton("Jump") && Time.time < lastGrounded + jumpHoldDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        /*if (isGrounded && Input.GetButton("Jump"))
        {
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }*/
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        moveSpeed = dashSpeed;

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // Reset speed after dash
        moveSpeed = originalMoveSpeed;
        isDashing = false;

        // Wait for dash cooldown
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
