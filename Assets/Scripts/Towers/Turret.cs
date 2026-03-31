using UnityEngine;
using System.Collections.Generic;

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
    public bool fireAllAtOnce = false;
    private int currentBarrelIndex = 0;

    private Transform target;

    void Start()
    {
        // Automatically find barrels if none were assigned manually
        if (turretBarrels.Count == 0)
        {
            turretBarrels.AddRange(GetComponentsInChildren<Barrel>());
        }

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = (nearestEnemy != null && shortestDistance <= range) ? nearestEnemy.transform : null;
    }

    void Update()
    {
        if (target == null) return;

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
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        foreach (Barrel barrel in turretBarrels)
        {
            if (barrel.pivot == null) continue;

            // Smoothly rotate the barrel's pivot toward the enemy
            barrel.pivot.rotation = Quaternion.Lerp(barrel.pivot.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }

    void Shoot()
    {
        if (turretBarrels.Count == 0) return;

        if (fireAllAtOnce)
        {
            foreach (Barrel barrel in turretBarrels)
            {
                FireFromBarrel(barrel);
            }
        }
        else
        {
            FireFromBarrel(turretBarrels[currentBarrelIndex]);
            currentBarrelIndex = (currentBarrelIndex + 1) % turretBarrels.Count;
        }
    }

    void FireFromBarrel(Barrel barrel)
    {
        // Check if the barrel or the firePoint reference has been lost
        if (barrel == null || barrel.firePoint == null || bulletPrefab == null) 
        {
            return; 
        }

        GameObject bulletGO = Instantiate(bulletPrefab, barrel.firePoint.position, barrel.firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}