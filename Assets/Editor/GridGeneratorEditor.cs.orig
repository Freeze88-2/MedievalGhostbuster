using UnityEditor;
using UnityEngine;
using AI.PathFinding.GridGeneration;

[CustomEditor(typeof(GridGenerator))]
[CanEditMultipleObjects]
public class GridGeneratorEditor : Editor
{
    private SerializedProperty unwalkablemask, areaSize, nodeRadius;

    private void OnEnable()
    {
        unwalkablemask = serializedObject.FindProperty("unwalkablemask");
        areaSize = serializedObject.FindProperty("areaSize");
        nodeRadius = serializedObject.FindProperty("nodeRadius");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Grid Generation options", MessageType.None);
        EditorGUILayout.PropertyField(unwalkablemask, true);
        EditorGUILayout.PropertyField(nodeRadius, true);
        EditorGUILayout.PropertyField(areaSize, true);

        serializedObject.ApplyModifiedProperties();

        GridGenerator gridScript = (GridGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            gridScript.StartGridGeneration();
        }
    }
}
