using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAHealthPotion : MonoBehaviour
{
    public int healthGain = 5;
    public bool collide = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collide = true;
            FAGameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FAGameManager>();
            if (gameManager != null)
            {
                gameManager.GainHealth(healthGain);
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
            Destroy(gameObject);  // Destroy the health potion after it is used
        }
    }
}