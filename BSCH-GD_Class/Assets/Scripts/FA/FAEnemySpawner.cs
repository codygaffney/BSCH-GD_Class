using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
