using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float stopDistance = 2.0f; // The "buffer" so enemies don't stack
    
    [Tooltip("Add X, Y, or Z rotation Offset")]
    public Vector3 rotationOffset; 

    private Transform currentWaypoint;
    private Transform closestTarget;
    
    private int waypointIndex = 0;
    private bool isAtTarget = false;

    void Start()
    {
        if (Waypoints.points != null && Waypoints.points.Length > 0)
        {
            currentWaypoint = Waypoints.points[0];
        }
        else
        {
            FindClosestTarget();
        }
    }

    void Update()
    {
        if (isAtTarget) return; 

        
        if (currentWaypoint != null)
        {
            MoveAndRotate(currentWaypoint.position);

            Vector3 targetPos = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);
            float distanceToWaypoint = Vector3.Distance(transform.position, targetPos);

            if (waypointIndex >= Waypoints.points.Length - 1 && distanceToWaypoint <= stopDistance)
            {
                isAtTarget = true;
                Debug.Log(gameObject.name + " Reached Final Waypoint!");
                return;
            }

            if (distanceToWaypoint <= 0.2f)
            {
                GetNextWaypoint();
            }
        }
        
        else
        {
            if (closestTarget == null)
            {
                FindClosestTarget();
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

    private void MoveAndRotate(Vector3 destination)
    {
        Vector3 targetPosition = new Vector3(destination.x, transform.position.y, destination.z);
        Vector3 dir = targetPosition - transform.position;

        // Standard Movement
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotation with the new offset applied!
        if (dir != Vector3.zero)
        {
            // Calculate where we SHOULD look, then add your custom offset
            Quaternion lookRotation = Quaternion.LookRotation(dir) * Quaternion.Euler(rotationOffset);
            
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
        if (GameManager.instance == null || GameManager.instance.targets == null) return;

        Target[] allTargets = GameManager.instance.targets;
        
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (Target t in allTargets)
        {
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
        if (GameManager.instance != null)
        {
            GameManager.instance.EnemyDestroyed();
        }
    }
}