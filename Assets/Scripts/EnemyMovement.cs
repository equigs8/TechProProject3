using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 10f;
    public float rotationSpeed = 5f;

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        // Start moving toward the first waypoint
        target = Waypoints.points[0];
    }

    void Update()
    {
        // 1. Create a "flattened" target position at the enemy's current height
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        
        // 2. Calculate direction based on the flattened position
        Vector3 dir = targetPosition - transform.position;
        
        // 3. Move towards the target (using the flattened direction)
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // 4. Rotate to face the direction
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // 5. Check distance (ignoring Y)
        if (Vector3.Distance(transform.position, targetPosition) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    void EndPath()
    {
        // Here you would deduct player lives
        Destroy(gameObject);
    }
}