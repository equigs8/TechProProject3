using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{

    public GameObject turretPrefab;

    private Button button;
    private TextMeshProUGUI costText;
    private Image backgroundImage; // Reference to the slot's background image

    // Cache the cost so we don't have to run expensive GetComponent calls every frame
    private int itemCost = 0;
    private bool costFound = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        backgroundImage = GetComponentInChildren<Image>();
        Debug.Log("Background Image: " + backgroundImage.name);
        if (button != null)
        {
            button.onClick.AddListener(SelectTower);
        }

        costText = GetComponentInChildren<TextMeshProUGUI>();
        
        // Find and save the cost immediately when the game starts
        itemCost = DetermineCost();
        UpdateCostText();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!costFound || ResourceManager.instance == null) return;

        // Check if the current oil is greater than or equal to the cached cost
        if (ResourceManager.instance.GetOil() >= itemCost)
        {
            backgroundImage.color = Color.white; // Normal color
            button.interactable = true;          // Make it clickable
        }
        else
        {
            backgroundImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Dark gray
            button.interactable = false;                             // Prevent clicking
        }
    }

    // Helper method to safely extract the price based on the prefab type
    int DetermineCost()
    {
        if (turretPrefab == null) return 0;

        Turret turret = turretPrefab.GetComponent<Turret>();
        if (turret != null)
        {
            costFound = true;
            return turret.GetPrice();
        }
        
        OilTank oilTank = turretPrefab.GetComponent<OilTank>();
        if (oilTank != null)
        {
            costFound = true;
            return oilTank.GetPrice(); 
        }
        
        ProductionBuilding prodBuilding = turretPrefab.GetComponent<ProductionBuilding>();
        if (prodBuilding != null)
        {
            costFound = true;
            return prodBuilding.GetPrice(); 
        }

        return 0;
    }

    void UpdateCostText()
    {
        // We can just use our cached itemCost here now to make it much cleaner!
        if (turretPrefab != null && costText != null && costFound)
        {
            costText.text = "$" + itemCost.ToString();
        }
    }

    public void SelectTower()
    {
        if(turretPrefab != null)
        {
            Debug.Log("Selected: " + turretPrefab.name);
            BuildManager.instance.SelectTurretToBuild(turretPrefab);
        }else
        {
            Debug.Log("No tower selected");
        }
    }
}