using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float stopDistance = 2.0f; // The "buffer" so enemies don't stack

    private Transform target;
    private int waypointIndex = 0;
    private bool isAtTarget = false;

    void Start()
    {
        target = Waypoints.points[0];
    }

    void Update()
    {
        if (isAtTarget) return; // Stop moving logic

        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 dir = targetPosition - transform.position;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Check if we are at the LAST waypoint and within buffer distance
        if (waypointIndex >= Waypoints.points.Length - 1 && distanceToTarget <= stopDistance)
        {
            isAtTarget = true;
            return;
        }

        // Standard Movement
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (distanceToTarget <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            // We have reached the final point; EnemyMovement Update handles the stop
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    public bool ReachedTarget() => isAtTarget;
}