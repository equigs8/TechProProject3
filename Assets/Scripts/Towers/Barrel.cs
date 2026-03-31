using UnityEngine;

public class Barrel : MonoBehaviour
{
    public Transform pivot;      // The part that actually rotates
    public Transform firePoint;  // Where the bullet comes out
    
    [HideInInspector]
    public Transform currentTarget; // Assigned by the Turret script
}