using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public int totalEnemiesPerStage = 20;
    public int maxEnemiesOnScreen = 4;
    public float spawnInterval = 3f;

    private int enemiesSpawned = 0;
    private int enemiesRemaining;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        enemiesRemaining = totalEnemiesPerStage;
        GameManager.Instance.UpdateEnemyCountUI(enemiesRemaining);
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (enemiesSpawned < totalEnemiesPerStage)
        {
            activeEnemies.RemoveAll(e => e == null);

            if (activeEnemies.Count < maxEnemiesOnScreen)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        activeEnemies.Add(enemy);
        enemiesSpawned++;
    }

    public void OnEnemyDestroyed()
    {
        enemiesRemaining--;
        GameManager.Instance.UpdateEnemyCountUI(enemiesRemaining);
    }
}