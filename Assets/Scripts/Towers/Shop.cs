using UnityEngine;

public class Shop : MonoBehaviour
{

    
    public GameObject standardTurret;
    

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        BuildManager.instance.SelectTurretToBuild(standardTurret);
    }
}
