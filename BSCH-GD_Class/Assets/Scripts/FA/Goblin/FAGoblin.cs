using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAGoblin : FABaseEnemy
{
    public Transform player;
    public bool isFlipped = false;

    public GameObject deathEffect;

    public bool isInvulnerable = false;
    public int attack1Damage = 10;
    public int attack2Damage = 15;

    public Vector2 knockbackForce;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    private FAGameManager gameManager;

    public override void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>();
        animator = this.GetComponent<Animator>();
        if (gameManager == null)
        {
            Debug.LogError("Failed to find the GameManager with FAGameManager component.");
        }
        base.Start();
    }

    public void Attack1()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.TakeDamage(attack1Damage);
            }
            else
            {
                Debug.LogError("GameManager not found or does not have FAGameManager component.");
            }
        }
    }

    public void Attack2()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>().TakeDamage(attack2Damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    public void LookAtPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackForce)
    {
        if (isInvulnerable)
            return;

        base.TakeDamage(damage, knockbackForce);
    }

    protected override void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameManager.IsGoblinDead = true;
        base.Die();
    }
}
