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

    public GameObject standardTurretPrefab;
    private GameObject turretToBuild;
    private GameObject previewInstance;

    public void SelectTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;

        if (turretToBuild.GetComponent<Turret>().GetPrice() >= ResourceManager.instance.GetOil())
        {
            Debug.Log("Not enough oil!");
            turretToBuild = null;
            return;
        }

        if (previewInstance != null) Destroy(previewInstance);

        if (turretToBuild != null)
        {
            previewInstance = Instantiate(turretToBuild);
            PreparePreview(previewInstance);
            ResourceManager.instance.RemoveOil(turretToBuild.GetComponent<Turret>().GetPrice());
        }
    }

    void PreparePreview(GameObject preview)
    {
        // Disable gameplay scripts and colliders on the ghost
        if (preview.TryGetComponent(out Turret turret)) turret.enabled = false;
        
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
        if (GameManager.instance.gameState != GameManager.GameState.BuildingPhase) 
        {
            if (GetTurretToBuild() != null) SelectTurretToBuild(null);
            return;
        }
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
        if (Physics.Raycast(ray, out RaycastHit hit))
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
            else previewInstance.SetActive(false);
        }
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Node node = hit.transform.GetComponent<Node>();
            if (node != null) BuildTurretOn(node);
        }
    }

    void BuildTurretOn(Node rootNode)
    {
        Vector2Int size = GetCurrentTurretSize();
        List<Node> footprint = GetNodesInFootprint(rootNode, size.x, size.y);

        if (footprint.Count < (size.x * size.y)) return;

        foreach (Node n in footprint)
        {
            if (n.turret != null) return;
        }

        Vector3 spawnPos = GetCenterOfNodes(footprint);
        GameObject turret = Instantiate(turretToBuild, spawnPos, Quaternion.identity);

        foreach (Node n in footprint) n.turret = turret;

        if (previewInstance != null) Destroy(previewInstance);
        turretToBuild = null;
    }

    List<Node> GetNodesInFootprint(Node rootNode, int w, int l)
    {
        List<Node> nodes = new List<Node>();
        string[] parts = rootNode.name.Split('_');
        if (parts.Length < 3) return nodes;

        int startX = int.Parse(parts[1]);
        int startZ = int.Parse(parts[2]);

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < l; z++)
            {
                GameObject neighbor = GameObject.Find($"Node_{startX + x}_{startZ + z}");
                if (neighbor != null && neighbor.TryGetComponent(out Node n))
                    nodes.Add(n);
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
        if (turretToBuild == null) return Vector2Int.one;
        return turretToBuild.GetComponent<Turret>()?.size ?? Vector2Int.one;
    }
    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
}