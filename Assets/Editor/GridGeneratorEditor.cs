using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the standard variables (width, length, etc.)
        DrawDefaultInspector();

        GridGenerator generator = (GridGenerator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Controls", EditorStyles.boldLabel);

        // Button to Generate
        if (GUILayout.Button("Generate Grid"))
        {
            // This line enables "Undo" functionality (Cmd + Z)
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Grid");
            
            generator.CreateGrid();
            
            // Mark the scene as 'dirty' so Unity knows to save the new objects
            EditorUtility.SetDirty(generator);
        }

        // Button to Clear
        if (GUILayout.Button("Clear Grid"))
        {
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Clear Grid");
            
            generator.ClearGrid();
            
            EditorUtility.SetDirty(generator);
        }
    }
}