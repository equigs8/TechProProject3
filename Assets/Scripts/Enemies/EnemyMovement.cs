using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float stopDistance = 2.0f; // The "buffer" so enemies don't stack

    // I split the target variable into two so the script knows 
    // if it's chasing a waypoint or an actual player Target
    private Transform currentWaypoint;
    private Transform closestTarget;
    
    private int waypointIndex = 0;
    private bool isAtTarget = false;

    void Start()
    {
        // 1. If waypoints exist, start following the path
        if (Waypoints.points != null && Waypoints.points.Length > 0)
        {
            currentWaypoint = Waypoints.points[0];
        }
        else
        {
            // 2. If no waypoints exist, immediately lock onto the closest of the 4 targets
            FindClosestTarget();
        }
    }

    void Update()
    {
        if (isAtTarget) return; // Stop moving logic

        // --- SCENARIO A: Following Waypoints ---
        if (currentWaypoint != null)
        {
            MoveAndRotate(currentWaypoint.position);

            Vector3 targetPos = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);
            float distanceToWaypoint = Vector3.Distance(transform.position, targetPos);

            // Check if we are at the LAST waypoint and within buffer distance
            if (waypointIndex >= Waypoints.points.Length - 1 && distanceToWaypoint <= stopDistance)
            {
                isAtTarget = true;
                Debug.Log(gameObject.name + " Reached Final Waypoint!");
                return;
            }

            // Check if we reached a middle waypoint and need to turn
            if (distanceToWaypoint <= 0.2f)
            {
                GetNextWaypoint();
            }
        }
        // --- SCENARIO B: Seeking Closest Target (No Waypoints) ---
        else
        {
            // If the target we were hunting was destroyed, find the next closest one
            if (closestTarget == null)
            {
                FindClosestTarget();
                
                // If it's STILL null, all 4 targets are destroyed. Stop moving.
                if (closestTarget == null) return; 
            }

            MoveAndRotate(closestTarget.position);

            Vector3 targetPos = new Vector3(closestTarget.position.x, transform.position.y, closestTarget.position.z);
            float distanceToTarget = Vector3.Distance(transform.position, targetPos);

            if (distanceToTarget <= stopDistance)
            {
                isAtTarget = true;
                Debug.Log(gameObject.name + " Reached a Target!");
            }
        }
    }

    // Consolidated your movement math here so it can be used for both scenarios
    private void MoveAndRotate(Vector3 destination)
    {
        Vector3 targetPosition = new Vector3(destination.x, transform.position.y, destination.z);
        Vector3 dir = targetPosition - transform.position;

        // Standard Movement
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotation
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1) return;

        waypointIndex++;
        currentWaypoint = Waypoints.points[waypointIndex];
    }

    private void FindClosestTarget()
    {
        // Failsafe in case GameManager isn't ready
        if (GameManager.instance == null || GameManager.instance.targets == null) return;

        Target[] allTargets = GameManager.instance.targets;
        
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (Target t in allTargets)
        {
            // Skip dead or missing targets
            if (t == null || t.health <= 0) continue; 

            float distanceToTarget = Vector3.Distance(transform.position, t.transform.position);
            
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTransform = t.transform;
            }
        }

        closestTarget = closestTransform;
    }

    public bool ReachedTarget() => isAtTarget;

    void OnDestroy()
    {
        // Check for instance to avoid errors when closing the game
        if (GameManager.instance != null)
        {
            GameManager.instance.EnemyDestroyed();
        }
    }
}