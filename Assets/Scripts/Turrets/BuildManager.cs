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
    
    // Track the currently highlighted nodes for the footprint
    private List<Node> currentHoveredNodes = new List<Node>();

    public void SelectTurretToBuild(GameObject turret)
    {
        ClearNodeHighlights(); 
        turretToBuild = turret;

        if (turretToBuild == null) return;
        if(turretToBuild.GetComponent<Turret>() != null){
            if (turretToBuild.GetComponent<Turret>().GetPrice() > ResourceManager.instance.GetOil())
            {
                Debug.Log("Not enough oil!");
                turretToBuild = null;
                return;
            }
        }else if (turretToBuild.GetComponent<OilTank>() != null)
        {
            if (turretToBuild.GetComponent<OilTank>().GetPrice() > ResourceManager.instance.GetOil())
            {
                Debug.Log("Not enough oil!");
                turretToBuild = null;
                return;
            }
        }else if (turretToBuild.GetComponent<ProductionBuilding>() != null)
        {
            if (turretToBuild.GetComponent<ProductionBuilding>().GetPrice() > ResourceManager.instance.GetOil())
            {
                Debug.Log("Not enough oil!");
                turretToBuild = null;
                return;
            }
        }
        

        if (previewInstance != null) Destroy(previewInstance);

        // Get specific offset for this turret prefab
        float yOffset = GetCurrentTurretOffset();
        Vector3 offset = new Vector3(0, yOffset, 0);
        
        previewInstance = Instantiate(turretToBuild, transform.position + offset, Quaternion.identity);
        PreparePreview(previewInstance);
        if (turretToBuild.GetComponent<Turret>() != null){
            ResourceManager.instance.RemoveOil(turretToBuild.GetComponent<Turret>().GetPrice());
        }else if (turretToBuild.GetComponent<OilTank>() != null){
            ResourceManager.instance.RemoveOil(turretToBuild.GetComponent<OilTank>().GetPrice());
        }else if (turretToBuild.GetComponent<ProductionBuilding>() != null){
            ResourceManager.instance.RemoveOil(turretToBuild.GetComponent<ProductionBuilding>().GetPrice());
        }
        
    }

    void PreparePreview(GameObject preview)
    {
        if (preview.TryGetComponent(out Turret turret)) turret.enabled = false;
        
        if(preview.TryGetComponent(out OilTank tank)) tank.enabled = false;

        if(preview.TryGetComponent(out ProductionBuilding building)) building.enabled = false;

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
                    
                    // Apply offset to preview while moving
                    float yOffset = GetCurrentTurretOffset();
                    previewInstance.transform.position = GetCenterOfNodes(footprint) + new Vector3(0, yOffset, 0);
                    
                    UpdateNodeHighlights(footprint); 
                }
                else
                {
                    ClearNodeHighlights();
                }
            }
            else 
            {
                previewInstance.SetActive(false);
                ClearNodeHighlights();
            }
        }
        else 
        {
            previewInstance.SetActive(false);
            ClearNodeHighlights();
        }
    }

    void UpdateNodeHighlights(List<Node> newNodes)
    {
        if (currentHoveredNodes.Count == newNodes.Count)
        {
            bool isSame = true;
            for (int i = 0; i < newNodes.Count; i++)
            {
                if (currentHoveredNodes[i] != newNodes[i])
                {
                    isSame = false;
                    break;
                }
            }
            if (isSame) return;
        }

        ClearNodeHighlights();

        currentHoveredNodes = new List<Node>(newNodes);
        foreach (Node n in currentHoveredNodes)
        {
            n.SetHoverColor(true);
        }
    }

    void ClearNodeHighlights()
    {
        foreach (Node n in currentHoveredNodes)
        {
            if (n != null) n.SetHoverColor(false);
        }
        currentHoveredNodes.Clear();
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

        // Apply offset to the final placement!
        float yOffset = GetCurrentTurretOffset();
        Vector3 spawnPos = GetCenterOfNodes(footprint) + new Vector3(0, yOffset, 0);
        GameObject turret = Instantiate(turretToBuild, spawnPos, Quaternion.identity);

        foreach (Node n in footprint) n.turret = turret;

        if (previewInstance != null) Destroy(previewInstance);
        
        ClearNodeHighlights(); 
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
        if(turretToBuild.GetComponent<Turret>() != null)
        {
            return turretToBuild.GetComponent<Turret>()?.size ?? Vector2Int.one;
        }else if (turretToBuild.GetComponent<OilTank>() != null)
        {
            return turretToBuild.GetComponent<OilTank>()?.size ?? Vector2Int.one;
        }else if (turretToBuild.GetComponent<ProductionBuilding>() != null)
        {
            return turretToBuild.GetComponent<ProductionBuilding>()?.size ?? Vector2Int.one;
        }

        return Vector2Int.one;
    }
    

    float GetCurrentTurretOffset()
    {
        if (turretToBuild == null) return 0f;
        if (turretToBuild.GetComponent<Turret>() != null)
        {
            return turretToBuild.GetComponent<Turret>()?.placementYOffset ?? 0f;
        }else if (turretToBuild.GetComponent<OilTank>() != null)
        {
            return turretToBuild.GetComponent<OilTank>()?.placementYOffset ?? 0f;
        }else if (turretToBuild.GetComponent<ProductionBuilding>() != null)
        {
            return turretToBuild.GetComponent<ProductionBuilding>()?.placementYOffset ?? 0f;
        }

        return 0f;
    }
    
    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
}