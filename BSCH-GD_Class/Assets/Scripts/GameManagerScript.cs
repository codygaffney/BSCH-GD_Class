using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public float health;

    public float score;
    public Transform spawnPoint;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Start").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
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
            // Reset health for demonstration purposes
            health = 10;
            // Optionally respawn the player at the starting point
            RespawnPlayer();
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
}
