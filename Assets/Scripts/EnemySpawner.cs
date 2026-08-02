using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // order: Basic, Fast, Power, Armor
    public Transform[] spawnPoints;
    public int totalEnemiesPerStage = 10;
    public int maxEnemiesOnScreen = 4;
    public float spawnInterval = 3f;

    private float[] spawnWeights = new float[4] { 1f, 1f, 1f, 1f };

    private int enemiesSpawned = 0;
    private int enemiesRemaining;
    private List<GameObject> activeEnemies = new List<GameObject>();

    public void ConfigureForStage(int stageNumber)
    {
        if (stageNumber <= 7)
        {
            totalEnemiesPerStage = 10;
            maxEnemiesOnScreen = 4;
            spawnWeights = new float[] { 3f, 1f, 1f, 1f }; // Basic, Fast, Power, Armor - basic favored
        }
        else if (stageNumber <= 14)
        {
            totalEnemiesPerStage = 15;
            maxEnemiesOnScreen = 5;
            spawnWeights = new float[] { 1f, 1f, 1f, 1f }; // equal chance
        }
        else
        {
            totalEnemiesPerStage = 20;
            maxEnemiesOnScreen = 6;
            spawnWeights = new float[] { 1f, 1f, 1f, 3f }; // armor favored
        }
    }

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

    GameObject PickWeightedPrefab()
    {
        float total = 0f;
        foreach (float w in spawnWeights) total += w;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        for (int i = 0; i < spawnWeights.Length; i++)
        {
            cumulative += spawnWeights[i];
            if (roll <= cumulative)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0]; // fallback
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = PickWeightedPrefab();

        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        EnemyTank enemyScript = enemy.GetComponent<EnemyTank>();
        if (Random.value < 0.2f)
        {
            enemyScript.dropsPowerUp = true;
        }

        activeEnemies.Add(enemy);
        enemiesSpawned++;
    }

    public void OnEnemyDestroyed()
    {
        enemiesRemaining--;
        GameManager.Instance.UpdateEnemyCountUI(enemiesRemaining);
    }

    public void ResetSpawner()
    {
        StopAllCoroutines();
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        activeEnemies.Clear();
        enemiesSpawned = 0;
        enemiesRemaining = totalEnemiesPerStage;
        GameManager.Instance.UpdateEnemyCountUI(enemiesRemaining);
        StartCoroutine(SpawnLoop());
    }

    public void SetSpawnPoints(Transform[] points)
    {
        spawnPoints = points;
    }
}