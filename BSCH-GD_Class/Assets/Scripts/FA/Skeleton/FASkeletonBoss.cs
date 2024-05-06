using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FASkeletonBoss : FABaseEnemy
{
    public Transform player;
    public bool isFlipped = false;

    public int attack1Damage = 10;
    public int attack2Damage = 15;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    //public GameObject deathEffect;

    public bool isInvulnerable = false;

    private FAGameManager gameManager;

    public override void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
        Debug.Log("Attack1 triggered, Collider found: " + (colInfo != null ? colInfo.name : "None"));
        if (colInfo != null && colInfo.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                Debug.Log("Player hit, attempting to deal damage");
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

        if (health <= 200)
        {
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }
    }

    protected override void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameManager.IsSkeletonDead = true;
        base.Die();
    }
}
