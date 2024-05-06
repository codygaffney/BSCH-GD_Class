using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAGameManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Transform spawnPoint;

    public bool IsSkeletonDead = false;
    public bool IsGoblinDead = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Start").transform;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GainHealth(float amount)
    {
        if(health+amount >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Damage taken, current health: " + health);
        CheckHealth();
    }

    // Checks the player's health and resets if needed
    private void CheckHealth()
    {
        if (health <= 0)
        {
            health = maxHealth;
            RespawnPlayer();
            ResetEnemies();
        }
    }

    // Respawns the player at the designated spawn point
    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player object
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position; // Move the player to the spawn point
        }
    }

    public void ResetEnemies()
    {
        // Destroy all existing enemies
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }
        FindAnyObjectByType<FASkeletonBossSummon>().bossSummoned = false;

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        // Find all spawners and tell them to spawn a new enemy
        FAEnemySpawner[] spawners = FindObjectsOfType<FAEnemySpawner>();
        foreach (FAEnemySpawner spawner in spawners)
        {
            spawner.SpawnEnemy();
        }
    }
}
