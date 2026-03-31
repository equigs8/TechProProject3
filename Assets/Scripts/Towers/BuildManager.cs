using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    [Header("Prefabs")]
    public GameObject standardTurretPrefab;
    public GameObject largeTurretPrefab;
    
    private GameObject turretToBuild;
    private GameObject previewInstance;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SelectTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;

        if (previewInstance != null) Destroy(previewInstance);

        if (turretToBuild != null)
        {
            previewInstance = Instantiate(turretToBuild);
            PreparePreview(previewInstance);
        }
    }

    void PreparePreview(GameObject preview)
    {
        // Disable logic so the ghost doesn't shoot or collide
        Turret turretScript = preview.GetComponent<Turret>();
        if (turretScript != null) turretScript.enabled = false;

        Collider[] colliders = preview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders) col.enabled = false;

        Renderer[] renders = preview.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            foreach (Material m in r.materials)
            {
                Color c = m.color;
                c.a = 0.5f;
                m.color = c;
            }
        }
    }

    void Update()
    {
        if (turretToBuild == null) return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
        {
            HandlePlacement();
        }
    }

    void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Node node = hit.transform.GetComponent<Node>();
            if (node != null)
            {
                Vector2Int size = GetCurrentTurretSize();
                List<Node> footprint = GetNodesInFootprint(node, size.x, size.y);

                if (footprint.Count > 0)
                {
                    previewInstance.SetActive(true);
                    previewInstance.transform.position = GetCenterOfNodes(footprint);
                }
            }
            else
            {
                previewInstance.SetActive(false);
            }
        }
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Node node = hit.transform.GetComponent<Node>();
            if (node != null)
            {
                BuildTurretOn(node);
            }
        }
    }

    void BuildTurretOn(Node rootNode)
    {
        Vector2Int size = GetCurrentTurretSize();
        List<Node> footprint = GetNodesInFootprint(rootNode, size.x, size.y);

        // Validation: Ensure the full area is available
        if (footprint.Count < (size.x * size.y))
        {
            Debug.Log("Not enough space!");
            return;
        }

        foreach (Node n in footprint)
        {
            if (n.turret != null)
            {
                Debug.Log("Area is occupied!");
                return;
            }
        }

        // Build the turret at the center of the selected nodes
        Vector3 spawnPos = GetCenterOfNodes(footprint);
        GameObject turret = Instantiate(turretToBuild, spawnPos, Quaternion.identity);

        // Mark all nodes in the footprint as occupied
        foreach (Node n in footprint)
        {
            n.turret = turret;
        }

        // Cleanup
        if (previewInstance != null) Destroy(previewInstance);
        turretToBuild = null;
    }

    // Helper: Finds neighbors based on the "Node_X_Z" naming convention
    List<Node> GetNodesInFootprint(Node rootNode, int w, int l)
    {
        List<Node> nodes = new List<Node>();
        string[] parts = rootNode.name.Split('_'); //
        
        if (parts.Length < 3) return nodes;

        int startX = int.Parse(parts[1]);
        int startZ = int.Parse(parts[2]);

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < l; z++)
            {
                GameObject neighbor = GameObject.Find($"Node_{startX + x}_{startZ + z}");
                if (neighbor != null)
                {
                    Node n = neighbor.GetComponent<Node>();
                    if (n != null) nodes.Add(n);
                }
            }
        }
        return nodes;
    }

    Vector3 GetCenterOfNodes(List<Node> nodes)
    {
        Vector3 center = Vector3.zero;
        foreach (Node n in nodes) center += n.GetPlacementPosition();
        return center / nodes.Count;
    }

    Vector2Int GetCurrentTurretSize()
    {
        Turret t = turretToBuild.GetComponent<Turret>();
        return t != null ? t.gridSize : new Vector2Int(1, 1);
    }
}