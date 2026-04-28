using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject nodePrefab;
    public int width = 10;
    public int length = 10;
    public float spacing = 1.1f;

    // Generates the grid of nodes
    public void CreateGrid()
    {
        ClearGrid();

        if (nodePrefab == null)
        {
            Debug.LogError("GridGenerator: Please assign a Node Prefab!");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                // Calculate position relative to the generator's position
                float offsetX = (width - 1) * spacing / 2f;
                float offsetZ = (length - 1) * spacing / 2f;
                Vector3 pos = new Vector3(x * spacing - offsetX, 0, z * spacing - offsetZ) + transform.position;
                // Instantiate using the prefab
                GameObject newNode = Instantiate(nodePrefab, pos, Quaternion.identity);
                
                // Parent it to this object to keep the hierarchy clean
                newNode.transform.parent = transform;
                newNode.name = $"Node_{x}_{z}";
            }
        }
    }

    // Removes all existing children (nodes)
    public void ClearGrid()
    {
        // Must iterate backwards when destroying objects in the editor
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}