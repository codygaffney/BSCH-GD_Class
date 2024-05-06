using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FASkeletonBossSummon : MonoBehaviour
{
    
    public GameObject skeletonBossPrefab; // Assign this in the inspector
    public bool bossSummoned = false;    // To ensure the boss is only summoned once

    public delegate void BossSummonedHandler(GameObject boss);
    public event BossSummonedHandler OnBossSummoned;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player and the boss has not been summoned yet
        if (other.CompareTag("Player") && !bossSummoned)
        {
            // Summon the boss 5 units to the right of the player
            Vector3 spawnPosition = other.transform.position + new Vector3(5, 0, 0);
            GameObject boss = Instantiate(skeletonBossPrefab, spawnPosition, Quaternion.identity);
            bossSummoned = true; // Set to true so the boss can't be summoned again

            OnBossSummoned?.Invoke(boss);
        }
    }
}
