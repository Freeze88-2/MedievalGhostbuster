using UnityEditor;
using UnityEngine;
using AI.PathFinding.GridGeneration;

[CustomEditor(typeof(GridGenerator))]
[CanEditMultipleObjects]
public class GridGeneratorEditor : Editor
{
    private SerializedProperty unwalkablemask, gridWorldSize, nodeRadius;

    private void OnEnable()
    {
        unwalkablemask = serializedObject.FindProperty("unwalkablemask");
        gridWorldSize = serializedObject.FindProperty("gridWorldSize");
        nodeRadius = serializedObject.FindProperty("nodeRadius");
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
