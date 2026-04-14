using UnityEngine;

public class OilTank : MonoBehaviour
{
    [Header("Attributes")]
    public int cost = 10;
    public int maxIncrease = 200;

     public ResourceManager resourceManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     resourceManager.IncreaseMax(maxIncrease);

    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
