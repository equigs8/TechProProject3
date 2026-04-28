using UnityEngine;

public class OilTank : MonoBehaviour
{
    [Header("Attributes")]
    public Vector2Int size = new Vector2Int(1, 1);
    public int cost = 10;
    public int maxIncrease = 200;

    public float placementYOffset = 0f;

     public ResourceManager resourceManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceManager = ResourceManager.instance;
        if (resourceManager == null) return;
        resourceManager.IncreaseMax(maxIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPrice()
    {
        return cost;
    }



}
