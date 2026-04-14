using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Turret : MonoBehaviour
{
    [Header("Placement Settings")]
    public Vector2Int size = new Vector2Int(1, 1); // Width (X) and Length (Y) in nodes

    [Header("Attributes")]
    public int cost = 10;
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
        if (turretBarrels.Count == 0)
            turretBarrels.AddRange(GetComponentsInChildren<Barrel>());

        InvokeRepeating("UpdateTargetAssignments", 0f, 0.5f);
    }

    void UpdateTargetAssignments()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        List<Transform> sortedEnemies = allEnemies
            .Select(e => e.transform)
            .Where(t => Vector3.Distance(transform.position, t.position) <= range)
            .OrderBy(t => Vector3.Distance(transform.position, t.position))
            .ToList();

        for (int i = 0; i < turretBarrels.Count; i++)
        {
            if (i < sortedEnemies.Count)
                turretBarrels[i].currentTarget = sortedEnemies[i];
            else
                turretBarrels[i].currentTarget = null;
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
            if (barrel.currentTarget == null || barrel.pivot == null) continue;

            Vector3 dir = barrel.currentTarget.position - barrel.pivot.position;
            if (dir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                barrel.pivot.rotation = Quaternion.Slerp(barrel.pivot.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
    }

    void Shoot()
    {
        foreach (Barrel barrel in turretBarrels)
        {
            if (barrel.currentTarget != null)
                FireFromBarrel(barrel);
        }
    }

    void FireFromBarrel(Barrel barrel)
    {
        if (barrel.firePoint == null || bulletPrefab == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, barrel.firePoint.position, barrel.firePoint.rotation);
        // Assuming your Bullet script has a Seek method
        bulletGO.GetComponent<Bullet>()?.Seek(barrel.currentTarget);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    internal int GetPrice()
    {
        return cost;
    }
}