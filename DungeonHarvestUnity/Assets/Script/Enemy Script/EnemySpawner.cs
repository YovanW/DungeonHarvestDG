using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 20f;
    public int maxEnemies = 10;
    public float spawnInterval = 0.5f;

    [Header("Debug")]
    public int currentEnemies = 0;

    private float nextSpawnTime;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPoint;

        if (!TryGetRandomNavMeshPoint(transform.position, spawnRadius, out spawnPoint))
            return;

        GameObject prefab =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(prefab, spawnPoint, Quaternion.identity);

        aliveEnemies.Add(enemy);
        currentEnemies++;

        EnemyLifeTracker tracker = enemy.AddComponent<EnemyLifeTracker>();
        tracker.spawner = this;
    }

    bool TryGetRandomNavMeshPoint(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 random = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(random, out hit, 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    public void OnEnemyDestroyed(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
        currentEnemies--;
    }
}
