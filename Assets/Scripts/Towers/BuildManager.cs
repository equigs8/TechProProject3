using UnityEngine;

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
    private GameObject previewInstance; // The ghost turret

    public void SelectTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
        
        // Clear old preview if selecting a new type
        if (previewInstance != null) Destroy(previewInstance);
        
        // Create the new preview ghost
        if (turretToBuild != null)
        {
            previewInstance = Instantiate(turretToBuild);
            PreparePreview(previewInstance);
        }
    }

    // Helper to make the preview look like a ghost
    void PreparePreview(GameObject preview)
    {
        // 1. Disable Colliders so Raycast doesn't flicker
        Collider[] colliders = preview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders) col.enabled = false;

        // 2. ONLY disable the Turret script so it doesn't try to shoot
        Turret turretScript = preview.GetComponent<Turret>();
        if (turretScript != null) turretScript.enabled = false;

        // 3. Make it transparent
        Renderer[] renders = preview.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            foreach (Material m in r.materials)
            {
                // Ensure your material's Rendering Mode is set to 'Fade' or 'Transparent'
                Color c = m.color;
                c.a = 0.5f; 
                m.color = c;
            }
        }
    }

    void Update()
    {
        UpdatePreviewPosition();

        if (turretToBuild == null) return;

        if (Input.GetMouseButtonDown(0))
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
    }

    [Header("Settings")]
    public LayerMask m_LayerMask; // Set this to everything EXCEPT the "Ignore Raycast" layer

    void UpdatePreviewPosition()
    {
        if (previewInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Add the m_LayerMask here to ignore the ghost
        if (Physics.Raycast(ray, out hit, 100f, m_LayerMask))
        {
            Node node = hit.transform.GetComponent<Node>();
            if (node != null)
            {
                previewInstance.SetActive(true);
                previewInstance.transform.position = node.GetPlacementPosition();
            }
            else
            {
                previewInstance.SetActive(false);
            }
        }
    }

    void BuildTurretOn(Node node)
    {
        if (node.turret != null)
        {
            Debug.Log("Node already occupied!");
            return;
        }

        Instantiate(turretToBuild, node.GetPlacementPosition(), Quaternion.identity);
        node.turret = turretToBuild;

        // Cleanup preview after building
        Destroy(previewInstance);
        turretToBuild = null;
    }
}