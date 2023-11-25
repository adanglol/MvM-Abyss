using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;  // Adjust this to set the player's movement speed
    public float jumpForce = 20f; // Adjust this to set the player's jump force
    public float jumpHoldDuration = 0.5f; //how long the player is holding the jump button
    public float dashSpeed = 30f; // Adjust this to set the dash speed
    public float dashDuration = 1f; // Adjust this to set the dash duration
    public float dashCooldown = 0.5f; // Adjust this to set the dash cooldown
    public float wallSlidingSpeed = 2f; //speed at which player slides down walls

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(100f, 30f);

    private Rigidbody2D rb;

    private bool isGrounded;
    private bool isDashing;
    private bool isWallSliding = false;
    private float originalMoveSpeed;
    private bool canDash = true;

    //ground and wall checks
    private Transform groundCheck;
    private LayerMask groundLayer;
    private Transform wallCheck;
    private LayerMask wallLayer;

    private float lastGrounded;

    private float moveInput;
    private bool isFacingRight = true;

    private Vector2 move;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck"); // Create an empty GameObject called "GroundCheck" slightly below the player
        groundLayer = LayerMask.GetMask("Ground"); // Make sure your ground objects are on the "Ground" layer
        wallCheck = transform.Find("WallCheck"); //Check for wall slide/jump
        wallLayer = LayerMask.GetMask("Ground"); //uses the ground layer for now
        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);


        // Apply dash if not currently dashing
        if (canDash && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        if (!isWallJumping) {
            FlipCharSprite();
        }

    }

    private void FixedUpdate() {
        // Player input for movement
        moveInput = Input.GetAxis("Horizontal");
        if (!isWallJumping) {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            //move = new Vector2(moveInput, 0f);
        }

        // If dashing, move with dash speed
        if (isDashing) {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
        }
        else {
            // Otherwise, move with regular speed
            rb.gravityScale = 4;
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }


        if (isGrounded) lastGrounded = Time.time;

        // Player input for jumping

        if (Input.GetButton("Jump") && Time.time < lastGrounded + jumpHoldDuration) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsWalled() {

        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide() {
        if (IsWalled() && !isGrounded && moveInput != 0f) {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        } else {
            isWallSliding = false;
        }
    }

    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        } else {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection) {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                if (isFacingRight) {
                    localScale.x = 0.7f;
                }
                else {
                    localScale.x = -0.7f;
                }

                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping() {
        isWallJumping = false;
    }

    private void FlipCharSprite() {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            if (isFacingRight) {
                localScale.x = 0.7f;
            } else {
                localScale.x = -0.7f;
            }
            
            transform.localScale = localScale;
        }
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
