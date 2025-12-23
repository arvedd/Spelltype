using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Minimum jumlah enemy yang spawn")]
    public int minEnemies = 1;

    [Tooltip("Maximum jumlah enemy yang spawn")]
    public int maxEnemies = 3;

    [Header("Enemy Pool")]
    [Tooltip("List musuh yang bisa spawn (random pick dari sini)")]
    public EnemySpawnData[] enemyPool;

    [Header("Spawn Positions")]
    [Tooltip("Posisi spawn untuk enemies")]
    public Transform[] spawnPoints;

    public Transform enemyContainer;
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        ClearEnemies();

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        Debug.Log($"Spawning {enemyCount} enemies");

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        if (availablePoints.Count == 0)
        {
            Debug.LogWarning("Tidak ada spawn point!");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            if (availablePoints.Count == 0)
            {
                Debug.LogWarning("Spawn point habis, tidak bisa spawn lebih banyak musuh");
                break;
            }


            GameObject enemyPrefab = PickRandomEnemy();

            if (enemyPrefab == null)
            {
                continue;
            }

            // Vector3 spawnPos = GetSpawnPosition(i);

            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[randomIndex];
            availablePoints.RemoveAt(randomIndex);

            GameObject enemyObj = Instantiate(enemyPrefab, chosenPoint.position, Quaternion.identity);
            Enemy enemy = enemyObj.GetComponentInChildren<Enemy>();

            if (enemy != null)
            {
                spawnedEnemies.Add(enemy);
                enemy.OnEnemyDeath += HandleEnemyDeath;
            }
        }

    }

    GameObject PickRandomEnemy()
    {
        if (enemyPool == null || enemyPool.Length == 0)
        {
            return null;
        }

        float totalWeight = 0f;
        foreach (var enemy in enemyPool)
        {
            totalWeight += enemy.spawnWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in enemyPool)
        {
            cumulativeWeight += enemy.spawnWeight;

            if (randomValue <= cumulativeWeight)
            {
                return enemy.enemyPrefab;
            }
        }

        return enemyPool[0].enemyPrefab;
    }

    Vector3 GetSpawnPosition(int index)
    {

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            return spawnPoints[index % spawnPoints.Length].position;
        }

        return new Vector3(2f + (index * 1.5f), 0f, 0f);
    }

    void HandleEnemyDeath(Enemy deadEnemy)
    {
        spawnedEnemies.Remove(deadEnemy);

        if (spawnedEnemies.Count == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    void OnAllEnemiesDefeated()
    {
        FindAnyObjectByType<BattleSystem>().CheckIfDied();
    }

    public void ClearEnemies()
    {
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                enemy.OnEnemyDeath -= HandleEnemyDeath;
                Destroy(enemy.gameObject);
            }
        }

        spawnedEnemies.Clear();

        foreach (Transform child in enemyContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public int GetAliveEnemyCount()
    {
        return spawnedEnemies.Count;
    }
    
    public List<Enemy> GetAliveEnemies()
    {
        Debug.Log(spawnedEnemies);
        return new List<Enemy>(spawnedEnemies);
    }

}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    
    [Range(0f, 1f)]
    public float spawnWeight = 1f;
}


