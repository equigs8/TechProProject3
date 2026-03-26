using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    private Transform target; // Reference to the target, which will be the last waypoint.

    void Awake()
    {
        target = GameObject.Find("Target").transform; 

        points = new Transform[transform.childCount + 1];
        for (int i = 0; i < points.Length - 1; i++)
        {
            points[i] = transform.GetChild(i);
        }
        if(target != null){
            points[points.Length - 1] = target;
            Debug.Log("Target found! " + target.name);
        }else{
            Debug.Log("Target not found!");
        }
    }

    private void OnDrawGizmos()
    {
        
        // Check if there are any waypoints to draw
        if (transform.childCount == 0) return;

        if (target == null)
        {
            target = FindFirstObjectByType<Target>().transform;
        }
       

        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.blue; // Choose a high-visibility color
            Transform currentPoint = transform.GetChild(i);
            
            // 1. Draw a sphere at every waypoint position
            Gizmos.DrawSphere(currentPoint.position, 0.4f);

            // 2. Draw a line to the next waypoint
            if (i < transform.childCount - 1)
            {
                Transform nextPoint = transform.GetChild(i + 1);
                Gizmos.DrawLine(currentPoint.position, nextPoint.position);
            }
            

            else if (target != null)
            {
                Gizmos.DrawLine(currentPoint.position, target.position);
                
                // Draw a special sphere for the tower so you know it's the end
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(target.position, 0.6f);
                Gizmos.color = Color.cyan; 
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(currentPoint.position, currentPoint.forward * 2f);
        }

        
    }
}