using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{

    [Header("Attributes")]
    public int cost = 1000;
    public int production = 5;

    public ResourceManager resourceManager;

    void Start()
    {
        
    }

    void Update()
    {
        resourceManager.AddOil(production);
        
    }
}
