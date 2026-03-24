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

    void OnDrawGizmos()
{
    for (int i = 0; i < transform.childCount; i++)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.GetChild(i).position, 0.3f);

        if (i < transform.childCount - 1)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }
}
}