using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{

    public GameObject turretPrefab;

    private Button button;
    private TextMeshProUGUI costText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(SelectTower);
        }

        costText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateCostText();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateCostText()
    {
        if (turretPrefab != null && costText != null)
        {
            
            Turret turret = turretPrefab.GetComponent<Turret>();
            
            if (turret != null)
            {
                costText.text = "$" + turret.GetPrice().ToString();
            }else if (turretPrefab.GetComponent<OilTank>() != null)
            {
                costText.text = "$" + turretPrefab.GetComponent<OilTank>().GetPrice().ToString();
            }else if (turretPrefab.GetComponent<ProductionBuilding>() != null)
            {
                costText.text = "$" + turretPrefab.GetComponent<ProductionBuilding>().GetPrice().ToString();
            }
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
