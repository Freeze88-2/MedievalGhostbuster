using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveController))]
public class SaveControllerInspector : Editor
{
    private SaveController _controller;

    private void OnEnable()
    {
        _controller = target as SaveController;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("", GUILayout.Height(3));

        if (GUILayout.Button("Find All", GUILayout.Height(25)))
        {
            _controller.GetAllSaveables();
            Debug.LogWarning("Finding all Saveable objects in scene...");
        }

        if (GUILayout.Button("Update", GUILayout.Height(25)))
        {
            _controller.UpdateUnsetIds();
            Debug.LogWarning("Updating inexistent IDs...");
        }
    }
}