using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoints))]
public class WaypointEditor : Editor
{
    private Waypoints script;

    private void OnEnable()
    {
        script = (Waypoints)target;
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        // Check if Shift + Left Click is pressed
        if (e.type == EventType.MouseDown && e.button == 0 && e.shift)
        {
            // Cast a ray from the mouse position into the 3D world
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CreateWaypoint(hit.point);
                // Consume the event so we don't select other objects
                e.Use();
            }
        }
    }

    void CreateWaypoint(Vector3 position)
    {
        GameObject go = new GameObject("Waypoint (" + script.transform.childCount + ")");
        go.transform.position = position;
        go.transform.parent = script.transform;

        // Mark scene as dirty so Unity knows to save the new objects
        EditorUtility.SetDirty(script);
        Undo.RegisterCreatedObjectUndo(go, "Create Waypoint");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Hold SHIFT + Left Click in Scene View to place waypoints on the floor.", MessageType.Info);
        
        if (GUILayout.Button("Clear All Waypoints"))
        {
            for (int i = script.transform.childCount - 1; i >= 0; i--)
            {
                Undo.DestroyObjectImmediate(script.transform.GetChild(i).gameObject);
            }
        }
    }
}