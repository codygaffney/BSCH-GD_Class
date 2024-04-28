using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABat : MonoBehaviour
{
    public float health = 5;
    public float speed = 3.0f;
    public float attackRange = 1.0f;
    public float attackDamage = 1.0f;
    public Transform player;
    public LayerMask playerLayer;
    public Animator animator;

    private Vector3 originalPosition;
    private Vector3 patrolTarget;
    private bool isDead = false;

    private State currentState;
    private bool shouldRetreat = false;
    private float retreatTimer = 0;

    public FAGameManager gameManager;

    private enum State
    {
        Patrolling,
        Attacking,
        Retreating
    }

    void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetNewPatrolTarget();
        currentState = State.Patrolling;
    }

    void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
            case State.Retreating:
                Retreat();
                break;
        }

        // Improved state transition logic
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange && currentState == State.Patrolling)
        {
            ChangeState(State.Attacking);
        }
        else if (shouldRetreat && currentState != State.Retreating)
        {
            ChangeState(State.Retreating);
        }
        else if (currentState == State.Retreating && retreatTimer <= 0)
        {
            ChangeState(State.Patrolling);
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        if (newState == State.Retreating)
        {
            shouldRetreat = false;
            retreatTimer = 3f; // Set retreat time
        }
    }

    private void SetNewPatrolTarget()
    {
        float randomDistance = Random.Range(3f, 7f) * (Random.Range(0, 2) * 2 - 1);
        patrolTarget = new Vector3(originalPosition.x + randomDistance, originalPosition.y, originalPosition.z);
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            SetNewPatrolTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        animator.SetBool("IsFlying", true);
    }

    private void AttackPlayer()
    {
        // Move towards the player to attack
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        retreatTimer -= Time.deltaTime;
        if (retreatTimer <= 0)
        {
            ChangeState(State.Patrolling);
        }
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
                shouldRetreat = true;
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<Rigidbody2D>().gravityScale = 1;
        // Optionally disable the bat's collider and logic after dying
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in  colliders)
        {
            collider.sharedMaterial = null;
        }
        this.enabled = false;
    }

    // Use this to apply damage to the player when in range and attacking
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentState == State.Attacking)
        {
            animator.SetTrigger("Attack");
            gameManager.TakeDamage(attackDamage); // Apply damage
            shouldRetreat = true;
            // Calculate the knockback force
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            float knockbackStrength = 5.0f; // Adjust this value as needed
            Vector2 knockbackForce = knockbackDirection * knockbackStrength;

            // Apply the knockback to the player
            collision.gameObject.GetComponent<FACharacterControllerScript>().GetHit(knockbackForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bat has hit the ground
        if (collision.gameObject.CompareTag("Ground") && isDead)
        {
            Destroy(gameObject); // Destroy the bat GameObject
        }
    }
}