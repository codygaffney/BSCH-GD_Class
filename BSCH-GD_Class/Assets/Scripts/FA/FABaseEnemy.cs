using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABaseEnemy : MonoBehaviour
{
    public float health;
    public float speed;
    private bool droppedHP = false;
    public Animator animator;
    public GameObject healthPotionPrefab;

    public virtual void TakeDamage(float damage, Vector2 knockbackForce)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Common reaction to taking damage can be handled here
            ApplyKnockback(knockbackForce);
        }
    }

    protected virtual void Die()
    {
        // Common death logic
        animator.SetTrigger("Die");
        if (!droppedHP)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            droppedHP = true;
        }
        StartCoroutine(AnimationWait());
    }

    private IEnumerator AnimationWait()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    protected void ApplyKnockback(Vector2 force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
