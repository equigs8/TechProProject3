using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

    private void OnDrawGizmos()
    {
        
        // Check if there are any waypoints to draw
        if (transform.childCount == 0) return;

        Gizmos.color = Color.cyan; // Choose a high-visibility color

        for (int i = 0; i < transform.childCount; i++)
        {
            
            Transform currentPoint = transform.GetChild(i);
            
            // 1. Draw a sphere at every waypoint position
            Gizmos.DrawSphere(currentPoint.position, 0.4f);

            // 2. Draw a line to the next waypoint
            if (i < transform.childCount - 1)
            {
                Transform nextPoint = transform.GetChild(i + 1);
                Gizmos.DrawLine(currentPoint.position, nextPoint.position);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawRay(currentPoint.position, currentPoint.forward * 2f);
        }
    }
}