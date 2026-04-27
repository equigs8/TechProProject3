using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("List enemies from weakest (Index 0) to strongest (Last Index)")]
    public GameObject[] enemies;

    [Header("Spawn Area")]
    [Tooltip("The radius around the spawner where enemies can appear.")]
    public float spawnRadius = 5f;
    public float spawnOffsetY = 1f;
    // Called by the GameManager when the Ready button is clicked
    public void StartWave(int waveLevel, int amountToSpawn)
    {
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogError("No enemies assigned to the Spawner!");
            return;
        }

        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnSingleEnemy(waveLevel);
        }
    }

    private void SpawnSingleEnemy(int currentLevel)
    {
        int indexToSpawn = GetWeightedSpawnIndex(currentLevel);
        
        // 1. By adding .normalized, we take a random point inside the circle 
        // and stretch it out so it sits perfectly on the 1-unit edge.
        // Then we multiply it by the radius to push it to your exact boundary.
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        
        // 2. Add that offset to the spawner's current position 
        Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, spawnOffsetY, randomPoint.y);

        // 3. Instantiate the selected enemy at the new randomized position
        Instantiate(enemies[indexToSpawn], spawnPosition, Quaternion.identity);
    }

    private int GetWeightedSpawnIndex(int currentLevel)
    {
        float[] weights = new float[enemies.Length];
        float totalWeight = 0f;

        for (int i = 0; i < enemies.Length; i++)
        {
            float weight = 0f;
            int minLevelToSpawn = i + 1; 

            if (currentLevel >= minLevelToSpawn)
            {
                weight = 10f + (currentLevel * i * 5f); 
            }
            else if (i == 0)
            {
                weight = 10f;
            }

            weights[i] = weight;
            totalWeight += weight;
        }

        float randomRoll = Random.Range(0f, totalWeight);
        float currentSum = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            currentSum += weights[i];
            if (randomRoll <= currentSum)
            {
                return i;
            }
        }

        return 0; 
    }

    private void OnDrawGizmosSelected()
    {
        // This will draw a colored wire sphere in the Scene view when you click on the Spawner
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}