using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.TakeDamage(1);
            }
            else
            {
                Debug.LogError("GameManagerScript not found on any GameObject.");
            }
        }
    }
}
