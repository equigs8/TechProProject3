using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{

    [Header("Attributes")]
    public int cost = 10;
    public int production = 1;

    public ResourceManager resourceManager;

    void Start()
    {
        
    }

    void Update()
    {
        resourceManager.AddOil(production);
        
    }
}
