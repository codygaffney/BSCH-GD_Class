using System.Collections;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        DetectPlayer,
        Chasing,
        AggroIdle,
    }

    public State enemyAIState = State.Idle;
    public Transform player;
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    private float speed;

    public float detectedPlayerTime = 2f;
    public float aggroTime = 5f;

    private bool playerDetected = false;
    private bool aggro = false;
    private Rigidbody2D _myRb;
    private Animator _animator;
    private Vector2 movement;

    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch (enemyAIState)
        {
            case State.Idle:
                speed = 0;
                _animator.SetBool("IsMoving", false);
                break;
            case State.Patrol:
                speed = moveSpeed;
                Patrol();
                _animator.SetBool("IsMoving", true);
                break;
            case State.DetectPlayer:
                speed = 0;
                _animator.SetBool("IsMoving", false);
                break;
            case State.Chasing:
                speed = chaseSpeed;
                ChasePlayer();
                _animator.SetBool("IsMoving", true);
                break;
            case State.AggroIdle:
                speed = 0;
                _animator.SetBool("IsMoving", false);
                break;
        }
        _myRb.velocity = movement * speed;
    }

    private void Patrol()
    {
        // Simple patrol logic, possibly switch directions at intervals or upon reaching barriers
        movement = Vector2.left;
    }

    private void ChasePlayer()
    {
        if (player)
        {
            // Calculate the normalized direction to the player
            movement = (player.position - transform.position).normalized;

            // Check the direction to the player and flip the enemy sprite accordingly
            // This assumes the enemy's default facing direction is to the right (localScale.x positive)
            if (movement.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);  // Face left
            else if (movement.x < 0)
                transform.localScale = new Vector3(1, 1, 1);   // Face right
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            enemyAIState = State.DetectPlayer;
            StartCoroutine(DetectTimer());
        }
    }

    IEnumerator DetectTimer()
    {
        yield return new WaitForSeconds(detectedPlayerTime);
        if (playerDetected)
        {
            aggro = true;
            enemyAIState = State.Chasing;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            StartCoroutine(AggroTimer());
        }
    }

    IEnumerator AggroTimer()
    {
        yield return new WaitForSeconds(aggroTime);
        aggro = false;
        enemyAIState = playerDetected ? State.Chasing : State.Idle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player similarly to how spikes do it
            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.TakeDamage(1);
            }
        }
    }
}
