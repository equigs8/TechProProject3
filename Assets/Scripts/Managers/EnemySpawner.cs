using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 20f;
    public float spawnInterval = 0.5f; // Time between individual spawns
    public int baseEnemyCount = 5; // Enemies in wave 1
    
    [Header("Unity Setup")]
    public Transform centerPoint;

    // Triggered by GameManager
    public void StartWave(int waveNumber)
    {
        int totalEnemies = baseEnemyCount * waveNumber; // Scaling logic
        GameManager.instance.enemiesAlive = totalEnemies;
        StartCoroutine(SpawnWaveRoutine(totalEnemies));
    }

    IEnumerator SpawnWaveRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * spawnRadius;
        float z = Mathf.Sin(angle) * spawnRadius;

        Vector3 spawnPos = new Vector3(x, 0, z) + (centerPoint != null ? centerPoint.position : transform.position);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = centerPoint != null ? centerPoint.position : transform.position;
        Gizmos.DrawWireSphere(center, spawnRadius);
    }
}