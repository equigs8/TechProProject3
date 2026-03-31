using UnityEngine;

public class Shop : MonoBehaviour
{

    
    public GameObject standardTurret;
    public GameObject largeTurret;
    

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        BuildManager.instance.SelectTurretToBuild(standardTurret);
    }

    public void SelectLargeTurret()
    {
        Debug.Log("Large Turret Selected");
        BuildManager.instance.SelectTurretToBuild(largeTurret);
    }
}
