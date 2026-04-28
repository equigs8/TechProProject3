using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject buildingPhaseUI;
    public GameObject gameOverUI;
    public GameObject resourceUI;
    internal void BuildingPhaseUI(bool v)
    {
        buildingPhaseUI.SetActive(v);
    }

    internal void GameOverUI(bool v)
    {
        gameOverUI.SetActive(v);
        buildingPhaseUI.SetActive(!v);
    }

    public void UpdateOil(int amount)
    {
        Debug.Log(resourceUI.transform.GetChild(0).GetChild(1).GetChild(0).name);
        resourceUI.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.ToString(); //Set Amount
    }

    public void UpdateOilMax(int amount)
    {
        resourceUI.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
