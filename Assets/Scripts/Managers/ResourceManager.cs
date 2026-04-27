using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public int oilAmount;
    public bool producing;

    public int oilMax;

    public int oilMaxDefault;
    
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

        if (oilMax < oilMaxDefault){ //make sure oilMax doesnt fall bellow the default
            oilMax = oilMaxDefault;
        }

        if (oilAmount > oilMax){ //stop oilAmount from increaseing above oilMax 
            oilAmount = oilMax;
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

    //Oil Max functions

     public void IncreaseMax(int amount)
    {
        oilMax += amount;
    }

    public void LowerMax(int amount)
    {
        oilMax -= amount;
    }

    internal int GetMaxOil()
    {
        return oilMax;
    }

    internal int GetCurrentOil()
    {
        return oilAmount;
    }
}
