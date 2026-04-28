using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpawnRadiusVisualizer : MonoBehaviour
{
    [Header("References")]
    public EnemySpawner enemySpawner;
    
    [Header("Settings")]
    public float radiusOffset = 0f; // Use this if you want the fog slightly wider than the exact spawn point
    public float swirlSpeed = 2f;   // How fast the ring rotates

    private ParticleSystem fogParticleSystem;
    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        // Grab the Particle System and its Shape module
        fogParticleSystem = GetComponent<ParticleSystem>();
        shapeModule = fogParticleSystem.shape;

        // Automatically find the spawner if you forgot to drag it in
        if (enemySpawner == null)
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
        }

        UpdateFogRadius();
    }

    void Update()
    {
        // Keep the radius updated in case it changes, and slowly rotate the ring
        UpdateFogRadius();
        transform.Rotate(Vector3.up * swirlSpeed * Time.deltaTime, Space.World);
    }

    void UpdateFogRadius()
    {
        if (enemySpawner != null)
        {
            // Lock the Particle System's Torus radius to the Spawner's radius
            shapeModule.radius = enemySpawner.spawnRadius + radiusOffset;
        }
    }
}