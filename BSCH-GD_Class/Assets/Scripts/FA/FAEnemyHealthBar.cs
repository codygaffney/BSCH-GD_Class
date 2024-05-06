using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FAEnemyHealthBar : MonoBehaviour
{
    public Image healthFill;
    public FABaseEnemy enemyHealth;

    // Method to initialize the health bar
    public void Initialize(FABaseEnemy enemy)
    {
        enemyHealth = enemy;
    }

    private void Update()
    {
        if (enemyHealth != null && healthFill != null)
        {
            healthFill.fillAmount = enemyHealth.health / enemyHealth.maxHealth;
        }
    }

    public void UpdateHealthBar()
    {
        if (enemyHealth != null && healthFill != null)
        {
            float healthPercent = enemyHealth.health / enemyHealth.maxHealth;
            healthFill.fillAmount = healthPercent;
            Debug.Log($"Updated Health Bar: {healthPercent * 100}%");
        }
        else
        {
            Debug.LogError("Health fill UI component not assigned!");
        }
    }
}
