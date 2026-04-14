using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public int oilAmount;
    public bool producing;
    
    public static ResourceManager instance;
    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (producing)
        {
            AddOil(1);
        }
    }

    public int GetOil()
    {
        return oilAmount;
    }

    public void AddOil(int amount)
    {
        oilAmount += amount;
    }

    public void RemoveOil(int amount)
    {
        oilAmount -= amount;
    }

    public void StartOilProduction()
    {
        producing = true;
    }
    public void StopOilProduction()
    {
        producing = false;
    }
}
