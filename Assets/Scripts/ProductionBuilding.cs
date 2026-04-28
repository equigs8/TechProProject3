using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{

    [Header("Attributes")]
    public Vector2Int size = new Vector2Int(1, 1);
    public int cost = 1000;
    public float placementYOffset = 0f;
    public int production = 5;

    public ResourceManager resourceManager;

    void Start()
    {
        resourceManager = ResourceManager.instance;
    }

    void Update()
    {
        resourceManager.AddOil(production);
        
    }

    public int GetPrice()
    {
        return cost;
    }
}
