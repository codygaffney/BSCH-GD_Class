using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FASkeletonBossWeapon : MonoBehaviour
{
    public int attack1Damage = 10;
    public int attack2Damage = 15;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    private FAGameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>();
        if (gameManager == null)
        {
            Debug.LogError("Failed to find the GameManager with FAGameManager component.");
        }
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
}
