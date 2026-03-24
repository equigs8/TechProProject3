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
        // 1. Move towards the target
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // 2. Rotate to face the direction of movement
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // 3. Check if we reached the waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
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