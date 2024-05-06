using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public event Action<GameObject> OnEnemySpawned;

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        OnEnemySpawned?.Invoke(enemy);  // Trigger the event with the spawned enemy
    }
}
