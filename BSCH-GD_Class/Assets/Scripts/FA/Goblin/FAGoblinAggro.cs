using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAGoblinAggro : MonoBehaviour
{
    private Animator goblinAnimator;

    private void Start()
    {
        // Find all spawners and subscribe to the correct one
        foreach (var spawner in FindObjectsOfType<FAEnemySpawner>())
        {
            if (spawner.enemyPrefab.GetComponent<FAGoblin>() != null)  // Check if the prefab is a goblin
            {
                spawner.OnEnemySpawned += HandleBossSummoned;
                break;  // Assuming only one goblin spawner exists
            }
        }
    }

    private void HandleBossSummoned(GameObject boss)
    {
        // Access the Animator from the summoned boss
        if (boss.GetComponent<FAGoblin>() != null)  // Verify it's the goblin
        {
            goblinAnimator = boss.GetComponent<Animator>();
            goblinAnimator.SetBool("IsRunning", true);  // Start running if needed
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && goblinAnimator != null)
        {
            goblinAnimator.SetBool("IsRunning", true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && goblinAnimator != null)
        {
            goblinAnimator.SetBool("IsRunning", false);
        }
    }
}
