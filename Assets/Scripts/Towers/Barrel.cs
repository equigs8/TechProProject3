using UnityEngine;

public class Barrel : MonoBehaviour
{

    public Transform firePoint;
    public Transform pivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivot = gameObject.transform;
        //if(firePoint == null) firePoint = transform.Find("FirePoint").gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
