using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject buildingPhaseUI;
    public GameObject gameOverUI;
    internal void BuildingPhaseUI(bool v)
    {
        buildingPhaseUI.SetActive(v);
    }

    internal void GameOverUI(bool v)
    {
        gameOverUI.SetActive(v);
        buildingPhaseUI.SetActive(!v);
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
