using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FogOfWarPlane : MonoBehaviour
{
    [Header("References")]
    public EnemySpawner enemySpawner;
    
    [Header("Settings")]
    public float outerRadius = 1000f; // How far the blackout fog reaches (make it huge)
    public int segments = 64; // How smooth the circle is

    private Mesh fogMesh;

    void Start()
    {
        fogMesh = new Mesh();
        fogMesh.name = "FogOfWarMesh";
        GetComponent<MeshFilter>().mesh = fogMesh;
        
        if (enemySpawner == null)
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
        }

        GenerateMesh();
    }

    void GenerateMesh()
    {
        if (enemySpawner == null) return;

        // The hole perfectly matches the spawner
        float innerRadius = enemySpawner.spawnRadius;
        
        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];

        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // Inner vertex (The edge of the hole)
            vertices[i * 2] = new Vector3(cos * innerRadius, 0, sin * innerRadius);
            
            // Outer vertex (The edge of the world)
            vertices[i * 2 + 1] = new Vector3(cos * outerRadius, 0, sin * outerRadius);

            if (i < segments)
            {
                int v = i * 2;
                int t = i * 6;

                // Create the faces that connect the inner circle to the outer circle
                triangles[t] = v;
                triangles[t + 1] = v + 2;
                triangles[t + 2] = v + 1;

                triangles[t + 3] = v + 2;
                triangles[t + 4] = v + 3;
                triangles[t + 5] = v + 1;
            }
        }

        fogMesh.vertices = vertices;
        fogMesh.triangles = triangles;
        fogMesh.RecalculateNormals(); // Fixes lighting
    }
}