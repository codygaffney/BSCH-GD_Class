using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABaseEnemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float speed;
    private bool droppedHP = false;
    public Animator animator;
    public GameObject healthPotionPrefab;

    public GameObject healthBarPrefab;  // Assign in the inspector
    private GameObject healthBarInstance;
    public FAEnemyHealthBar healthBarScript;

    public virtual void Start()
    {
        //Debug.Log("Healthbar Start");
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
            healthBarInstance.transform.SetParent(transform);  // Make the canvas a child of the enemy
            healthBarInstance.transform.localPosition = new Vector3(0, 2.0f, 0);  // Adjust this value to position the canvas above the enemy
            healthBarInstance.transform.localScale = new Vector3(.1f, .1f, .1f);

            // Set the scale and rotation to be standard since it's now a child
            healthBarInstance.transform.localRotation = Quaternion.identity;
            healthBarInstance.transform.localScale = Vector3.one;


            // Initialize the health bar script
            healthBarScript = healthBarInstance.GetComponentInChildren<FAEnemyHealthBar>();
            if (healthBarScript != null)
            {
                healthBarScript.Initialize(this);
            }
        }
    }

    public virtual void TakeDamage(float damage, Vector2 knockbackForce)
    {
        health -= damage;
        Debug.Log($"Damage Taken: {damage}, Remaining Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Common reaction to taking damage can be handled here
            ApplyKnockback(knockbackForce);
        }

        if (healthBarInstance != null)
        {
            healthBarScript.UpdateHealthBar();  // Directly update the health bar
        }
        else
        {
            Debug.LogError("Health bar script not found on health bar instance.");
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    protected virtual void OnDestroy()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
    }
}
