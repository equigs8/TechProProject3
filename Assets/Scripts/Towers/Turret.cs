using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Required for sorting enemies

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup")]
    public string enemyTag = "Enemy";
    public float turnSpeed = 10f;
    public GameObject bulletPrefab;

    [Header("Barrel Configuration")]
    public List<Barrel> turretBarrels = new List<Barrel>();

    void Start()
    {
        // Automatically fill the list if empty
        if (turretBarrels.Count == 0)
        {
            turretBarrels.AddRange(GetComponentsInChildren<Barrel>());
        }

        // Search for targets twice a second
        InvokeRepeating("UpdateTargetAssignments", 0f, 0.5f);
    }

    void UpdateTargetAssignments()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        // Find all enemies in range and sort them by distance to this turret
        List<Transform> sortedEnemies = allEnemies
            .Select(e => e.transform)
            .Where(t => Vector3.Distance(transform.position, t.position) <= range)
            .OrderBy(t => Vector3.Distance(transform.position, t.position))
            .ToList();

        // Assign a unique enemy to each barrel
        for (int i = 0; i < turretBarrels.Count; i++)
        {
            if (i < sortedEnemies.Count)
            {
                // Assign the i-th closest enemy to the i-th barrel
                turretBarrels[i].currentTarget = sortedEnemies[i];
            }
            else
            {
                // No more unique enemies available
                turretBarrels[i].currentTarget = null;
            }
        }
    }

    void Update()
    {
        RotateBarrels();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void RotateBarrels()
    {
        foreach (Barrel barrel in turretBarrels)
        {
            // Ensure the barrel has a target and a valid pivot to rotate
            if (barrel.currentTarget == null || barrel.pivot == null) continue;

            // Calculate direction to the specific target assigned to this barrel
            Vector3 dir = barrel.currentTarget.position - barrel.pivot.position;
            
            if (dir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                // Use Slerp for smoother tracking
                barrel.pivot.rotation = Quaternion.Slerp(barrel.pivot.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
    }

    void Shoot()
    {
        foreach (Barrel barrel in turretBarrels)
        {
            // Only fire if this specific barrel has a target
            if (barrel.currentTarget != null)
            {
                FireFromBarrel(barrel);
            }
        }
    }

    void FireFromBarrel(Barrel barrel)
    {
        if (barrel.firePoint == null || bulletPrefab == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, barrel.firePoint.position, barrel.firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(barrel.currentTarget); // Pass the barrel's unique target
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}