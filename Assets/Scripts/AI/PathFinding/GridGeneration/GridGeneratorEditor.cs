using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
[CanEditMultipleObjects]
public class GridGeneratorEditor : Editor
{
    SerializedProperty unwalkablemask, gridWorldSize, nodeRadius, grid;

    private void OnEnable()
    {
        unwalkablemask = serializedObject.FindProperty("unwalkablemask");
        gridWorldSize = serializedObject.FindProperty("gridWorldSize");
        nodeRadius = serializedObject.FindProperty("nodeRadius");
        grid = serializedObject.FindProperty("grid");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Grid Generation options", MessageType.None);
        EditorGUILayout.PropertyField(unwalkablemask, true);
        EditorGUILayout.PropertyField(nodeRadius, true);
        EditorGUILayout.PropertyField(gridWorldSize, true);

        serializedObject.ApplyModifiedProperties();

        GridGenerator gridScript = (GridGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            gridScript.StartGridGeneration();
        }
    }

}
