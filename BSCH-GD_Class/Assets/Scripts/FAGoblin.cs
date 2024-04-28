using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAGoblin : MonoBehaviour
{
    public GameObject gameManager;
    public float speed = 2.0f;
    public float attackRange = 1.0f;
    public float chaseTime = 2.0f;
    public Transform player;
    public Animator animator;

    private Vector3 startPosition;
    private float patrolLimit = 4.0f;
    private float chaseTimer;
    private bool isFacingRight = true;
    private bool isChasing = false;

    public float health = 20.0f;
    public int attackDamage = 5;

    private enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Returning
    }

    private State currentState;

    void Start()
    {
        startPosition = transform.position;
        currentState = State.Patrolling;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                break;
            case State.Returning:
                ReturnToStart();
                break;
        }

        CheckForPlayer();
    }

    private void Patrol()
    {
        float patrolTarget = startPosition.x + (isFacingRight ? patrolLimit : -patrolLimit);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolTarget, startPosition.y, startPosition.z), speed * Time.deltaTime);
        animator.SetBool("IsRunning", true);
        if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolLimit)
        {
            isFacingRight = !isFacingRight;
            Flip();
        }
    }

    private void Chase()
    {
        if (chaseTimer > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
            chaseTimer -= Time.deltaTime;
        }
        else
        {
            currentState = State.Attacking;
        }
    }

    private IEnumerator Attack(Collider2D collision)
    {
        animator.SetBool("IsRunning", false);
        Debug.Log("Attack Method");
        // Attack in the current facing direction
        animator.SetTrigger("Attack");

        // Wait for the attack animation/duration
        yield return new WaitForSeconds(1.0f); // Adjust time according to your attack animation duration

        // Flip to face the opposite direction and attack
        Flip();
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.0f); // Same as above, adjust time accordingly

        // Flip back to the original direction and perform the final attack
        Flip();
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.0f); // Adjust time as needed

        // After completing the attack sequence, change state as necessary, e.g., to Returning or Patrolling
        currentState = State.Returning;
    }

    private void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        if (transform.position == startPosition)
        {
            currentState = State.Patrolling;
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 10f && currentState == State.Patrolling) // Assuming 10 units is the detection range
        {
            currentState = State.Chasing;
            chaseTimer = chaseTime;
        }
        else if (distanceToPlayer > 10f && currentState == State.Chasing)
        {
            currentState = State.Returning;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState = State.Attacking;
        if (collision.gameObject.CompareTag("Player") && currentState == State.Attacking)
        {
            AttackPlayer(collision.gameObject, collision);
            gameManager.GetComponent<FAGameManager>().TakeDamage(attackDamage);
            // Calculate the knockback force
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            float knockbackStrength = 5.0f; // Adjust this value as needed
            Vector2 knockbackForce = knockbackDirection * knockbackStrength;

            // Apply the knockback to the player
            collision.gameObject.GetComponent<FACharacterControllerScript>().GetHit(knockbackForce);
        }
    }


    private void AttackPlayer(GameObject player, Collider2D collision)
    {
        // You would add more mechanics here such as an attack animation or sound
        Debug.Log("Goblin attacks the player!");
        StartCoroutine(Attack(collision));
    }

    public void TakeDamage(float damage, Vector2 knockbackForce)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Apply the knockback force to the Rigidbody2D
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(knockbackForce, ForceMode2D.Impulse);
                currentState = State.Returning;
            }
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Rigidbody2D>().gravityScale = 1;
        // Optionally disable the bat's collider and logic after dying
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.sharedMaterial = null;
        }
        this.enabled = false;
    }
}
