using UnityEngine;

public class FACharacterControllerScript : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the character
    public float jumpForce = 10.0f; // Jump force
    public float dashSpeed = 15.0f; // Dash speed
    public float dashTime = 0.3f; // How long the dash lasts

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump; // To check if the player can still double jump
    private bool isDashing; // To check if the player is dashing
    private float dashTimeLeft; // Timer for how long the dash is active

    public Animator animator;
    private int comboStep = 0;
    private float comboTimer = 0.0f;
    private float comboTimeout = 1.0f; // Time in seconds before combo resets

    public float attackRange = 2.0f; // Range of the player's attack
    public float attackDamage = 2.0f;
    public LayerMask enemyLayerMask;

    public GameObject currentInteractableItem = null;
    public GameObject interactionPrompt;
    public GameObject getKeyPrompt;
    public bool canJump = false;
    public bool canDoubleJumpAbility = false; // To check if the player has acquired the double jump ability
    public bool canDash = false;
    public bool hasDJKey = false;
    public Collider2D playerCollider;

    public FAGameManager gameManager;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<FAGameManager>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleAttack();
        HandleDashing();

        // Handle item interaction
        if (Input.GetKeyDown(KeyCode.F) && currentInteractableItem != null)
        {
            HandleItemInteraction();
        }

        // Show or hide interaction prompt
        if (currentInteractableItem != null && ((currentInteractableItem.CompareTag("JumpBoostItem") && !canJump) || (currentInteractableItem.CompareTag("DoubleJumpItem") && !canDoubleJumpAbility)))
        {
            interactionPrompt.SetActive(true);
        }
        else if (currentInteractableItem != null && (currentInteractableItem.CompareTag("DoubleJumpItem") && !canDoubleJumpAbility && hasDJKey))
        {
            getKeyPrompt.SetActive(true);
        }
        else if (currentInteractableItem != null && (currentInteractableItem.CompareTag("DashItem") && !canDash))
        {
            getKeyPrompt.SetActive(true);
        }
        else
        {
            interactionPrompt.SetActive(false);
            getKeyPrompt.SetActive(false);
        }


        // Update the combo timer
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboTimeout)
            {
                ResetCombo(); // Reset the combo if the timer exceeds the timeout
            }
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Assume Mouse0 (left-click) is the attack key
        {
            Attack();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (!isDashing && Mathf.Abs(horizontalInput) > 0) // Only allow movement control when not dashing and there is horizontal input
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Handle flipping the character sprite based on movement direction
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJumping()
    {
        if (canJump && Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJumpAbility && canDoubleJump)))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity to ensure consistent jump force
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            if (isGrounded)
                canDoubleJump = true;  // Allow double jump after first jump
            else
                canDoubleJump = false; // Disable double jump after it's used

            animator.SetTrigger("Jump");
        }

        // Update the vertical animation state
        if (rb.velocity.y > 0)
            animator.SetBool("IsJumping", true);
        else if (rb.velocity.y < 0)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    private void HandleDashing()
    {
        if (canDash && Input.GetButtonDown("Dash") && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            float dashDirection = Mathf.Sign(rb.velocity.x);
            if (dashDirection == 0) // If not moving, default to facing right
                dashDirection = Mathf.Sign(transform.localScale.x);
            rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }

    void Attack()
    {
        comboTimer = 0.0f;
        TriggerAttackAnimation(); // Handles animations and combo logic

        Vector2 attackPoint = (Vector2)transform.position + Vector2.right * transform.localScale.x * 0.5f; // Adjust position as needed
        float attackRadius = 1.5f; // Adjust radius as needed

        // Check for enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRadius, enemyLayerMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            FABaseEnemy enemyComponent = enemy.GetComponent<FABaseEnemy>();
            if (enemyComponent != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                float knockbackStrength = 5.0f;
                Vector2 knockbackForce = knockbackDirection * knockbackStrength;
                enemyComponent.TakeDamage(attackDamage, knockbackForce);
            }
        }

        // Visualize the attack range in Scene view
        Debug.DrawRay(attackPoint, Vector2.right * transform.localScale.x * attackRadius, Color.red);
    }

    // This is a helper method to manage attack animations and combo logic
    void TriggerAttackAnimation()
    {
        if (comboStep == 0)
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackCombo", 1);
            comboStep = 1;
        }
        else if (comboStep == 1)
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackCombo", 2);
            comboStep = 2;
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackCombo", 3);
            comboStep = 3;
        }
        else
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackCombo", 1);
            comboStep = 1;
        }
    }

    // Call this method at the end of the last attack animation via an Animation Event
    public void ResetCombo()
    {
        comboStep = 0;
        comboTimer = 0.0f; // Also reset the timer when combo is reset
        animator.SetInteger("AttackCombo", 0);
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        // Disable player controls and handle game over logic
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false; // Reset double jump capability
            animator.SetBool("IsGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public void EnableDoubleJump()
    {
        canDoubleJumpAbility = true;
    }

    public void EnableDash()
    {
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect if the player enters the proximity of any special item chests
        if (collision.CompareTag("JumpBoostItem") || collision.CompareTag("DoubleJumpItem") || collision.CompareTag("DashItem"))
        {
            currentInteractableItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Clear the interactable item when the player exits its proximity
        if (collision.gameObject == currentInteractableItem)
        {
            currentInteractableItem = null;
        }
    }

    private void HandleItemInteraction()
    {
        if (currentInteractableItem == null) return;  // Exit if there's no item to interact with

        Animator chestAnimator = currentInteractableItem.GetComponent<Animator>();
        FAJumpChest chestScript = currentInteractableItem.GetComponent<FAJumpChest>();

        if (chestAnimator != null && chestScript != null)
        {
             

            // Check what type of item the chest gives and enable the corresponding ability
            if (currentInteractableItem.CompareTag("JumpBoostItem"))
            {
                chestAnimator.SetBool("IsActive", true);
                chestScript.OpenChest();
                EnableJump();
            }
            else if (currentInteractableItem.CompareTag("DoubleJumpItem"))
            {
                if (gameManager.IsGoblinDead)
                {
                    chestAnimator.SetBool("IsActive", true);
                    chestScript.OpenChest();
                    EnableDoubleJump();
                }
            }
            else if (currentInteractableItem.CompareTag("DashItem"))
            {
                if(gameManager.IsSkeletonDead)
                {
                    chestAnimator.SetBool("IsActive", true);
                    chestScript.OpenChest();
                    EnableDash();
                }
            }

            // Optionally clear the current interactable item if the interaction concludes it (depends on game design)
            // currentInteractableItem = null;
        }
    }

    public void GetHit(Vector2 knockbackForce)
    {
        // Apply the knockback force to the player's Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }
}
