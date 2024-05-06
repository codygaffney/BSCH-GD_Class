using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABat : FABaseEnemy
{
    public float attackRange = 1.0f;
    public float attackDamage = 1.0f;
    public Transform player;
    public LayerMask playerLayer;

    private Vector3 originalPosition;
    private Vector3 patrolTarget;
    private bool isDead = false;
    private State currentState;
    private float retreatTimer = 0f;
    private const float retreatDuration = 2f;

    public FAGameManager gameManager;

    private enum State
    {
        Patrolling,
        Attacking,
        Retreating
    }

    public override void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>();
        SetNewPatrolTarget();
        currentState = State.Patrolling;
        base.Start();
    }

    void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                CheckForPlayer();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
            case State.Retreating:
                Retreat();
                break;
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);
        animator.SetBool("IsFlying", true);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            SetNewPatrolTarget();
        }
    }

    private void AttackPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            ChangeState(State.Patrolling);
        }
    }

    private void Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        retreatTimer -= Time.deltaTime;

        if (retreatTimer <= 0)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(State.Attacking);
            }
            else
            {
                ChangeState(State.Patrolling);
            }
        }
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            ChangeState(State.Attacking);
        }
    }

    private void SetNewPatrolTarget()
    {
        patrolTarget = originalPosition + new Vector3(Random.Range(-5f, 5f), 0, 0);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case State.Attacking:
                animator.SetTrigger("Attack");
                break;
            case State.Retreating:
                retreatTimer = retreatDuration;
                break;
            case State.Patrolling:
                animator.SetBool("IsFlying", true);
                SetNewPatrolTarget();
                break;
        }
    }

    public override void TakeDamage(float damage, Vector2 knockbackForce)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            ChangeState(State.Retreating);
        }
    }


    protected override void Die()
    {
        base.Die(); // Calls the common death logic
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    // Use this to apply damage to the player when in range and attacking
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentState == State.Attacking)
        {
            animator.SetTrigger("Attack");
            gameManager.TakeDamage(attackDamage); // Apply damage
            ChangeState(State.Retreating);
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