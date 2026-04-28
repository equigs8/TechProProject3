using System;
using System.Collections;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public float oilRate;
    public int oilFlow;
    public int oilAmount;
    public bool producing;
    public int additionalFlow;

    public int oilMax;

    public int oilMaxDefault;
    private bool produceOilCoroutineRunning;
    
    public static ResourceManager instance;
    
    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    
    void Start()
    {
        oilMax = oilMaxDefault;
        oilAmount = oilMax;
    }

    // Update is called once per frame
    void Update()
    {
        // FIX 1: Check the running flag so we only start ONE coroutine
        if (producing && !produceOilCoroutineRunning)
        {
            StartCoroutine(ProduceOil()); // Must use StartCoroutine()
        }

        if (oilMax < oilMaxDefault){ //make sure oilMax doesnt fall bellow the default
            oilMax = oilMaxDefault;
        }

        if (oilAmount > oilMax){ //stop oilAmount from increaseing above oilMax 
            oilAmount = oilMax;
        }
    }

    IEnumerator ProduceOil()
    {
        Debug.Log("Producing oil");
        
        produceOilCoroutineRunning = true; 

        
        while (producing) 
        {
            Debug.Log("Inside While Loop");
            yield return new WaitForSeconds(oilRate);
            AddOil(oilFlow + additionalFlow);
        }
        
        
        produceOilCoroutineRunning = false; 
    }

    public int GetOil()
    {
        return oilAmount;
    }

    public void AddOil(int amount)
    {
        if(producing){
            oilAmount += amount;
        }
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

    public void AddAdditionalFlow(int amount)
    {
        additionalFlow += amount;
    }
    
    public void RemoveAdditionalFlow(int amount)
    {
        additionalFlow -= amount;
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