using System.Collections;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float acceleration = 50f;
    public float sprintMultiplier = 1.5f;
    private bool isSprinting = false;

    [Header("Jumping")]
    public float jumpForce = 300f;
    private int jumpCount = 0;
    public int maxJumpCount = 2;  // Allows for one jump and one double jump

    [Header("Dashing")]
    public float dashForce = 15f;
    public float dashDuration = 0.3f;
    private bool isDashing = false;
    private bool canDash = true;  // Ensure only one dash until reset

    [Header("Components")]
    public Rigidbody2D myRb;
    public Animator anim;

    private bool isGrounded;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleJump();
        HandleDash();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        if (isDashing)
            return;

        float inputX = Input.GetAxis("Horizontal");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Adjust speed for sprinting
        float speed = isSprinting ? maxSpeed * sprintMultiplier : maxSpeed;

        // Apply horizontal movement
        if (Mathf.Abs(inputX) > 0.1f && Mathf.Abs(myRb.velocity.x) < speed)
        {
            myRb.AddForce(new Vector2(inputX * acceleration, 0), ForceMode2D.Force);
        }

        // Update animator speed
        anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

        // Handle character flipping
        if (inputX > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (inputX < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < maxJumpCount)
            {
                myRb.velocity = new Vector2(myRb.velocity.x, 0); // Reset vertical velocity
                myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                anim.SetTrigger(jumpCount == 0 ? "Jump" : "DoubleJump");
                jumpCount++;
                isGrounded = false;  // Make sure we set isGrounded to false on jump
            }
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isDashing && canDash)
        {
            StartCoroutine(PerformDash());
            canDash = false;  // Disable further dashing until reset
        }
    }

    IEnumerator PerformDash()
    {
        isDashing = true;
        float originalGravity = myRb.gravityScale;
        myRb.gravityScale = 0;
        myRb.velocity = new Vector2(transform.localScale.x * dashForce, 0); // Dash in the facing direction

        yield return new WaitForSeconds(dashDuration);

        myRb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isGrounded)
        {
            isGrounded = true;
            jumpCount = 0; // Reset jump count when grounded
            canDash = true; // Reset dash ability
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

    private void UpdateAnimations()
    {
        // Handle fall animation
        if (!isGrounded && myRb.velocity.y < 0)
        {
            anim.SetBool("Fall", true);
        }
        else
        {
            anim.SetBool("Fall", false);
        }
    }
}
