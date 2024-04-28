using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the Player
        {
            Debug.Log("Player has hit the spike trap!"); // Log or handle the death
            Destroy(other.gameObject); // This line destroys the player object; replace with your death handling logic
        }
    }
}