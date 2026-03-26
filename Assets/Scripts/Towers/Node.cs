using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    [Header("Optional")]
    public GameObject turret; // Stores the turret currently on this node

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

    // Visual feedback when dragging over a node
    void OnMouseEnter()
    {
        // Debug.DrawRay(transform.position, Vector3.up * 5, Color.green, 2f);

        // Debug.Log("Mouse entered node: " + gameObject.name);
        // Only highlight if the player is currently "holding" a turret to place
        //if (BuildManager.instance.GetTurretToBuild() == null) return;
        
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}