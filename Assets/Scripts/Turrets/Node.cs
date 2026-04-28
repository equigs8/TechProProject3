using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    [Header("Optional")]
    public GameObject turret; 

    private Renderer rend;
    private Color startColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public Vector3 GetPlacementPosition()
    {
        return transform.position + positionOffset;
    }

    // Called by the BuildManager to highlight multi-node footprints
    public void SetHoverColor(bool isHovering)
    {
        if (isHovering)
            rend.material.color = hoverColor;
        else
            rend.material.color = startColor;
    }

    void OnMouseEnter()
    {
        // Don't do standard single-node hovering if we are placing a tower
        if (BuildManager.instance != null && BuildManager.instance.GetTurretToBuild() != null) return;
        
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        // Don't do standard single-node hovering if we are placing a tower
        if (BuildManager.instance != null && BuildManager.instance.GetTurretToBuild() != null) return;

        rend.material.color = startColor;
    }
}