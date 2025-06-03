using UnityEditor;
using UnityEngine;

public class GraphInteraction : EditorWindow
{

    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<GraphInteraction>("Graph Interaction");
    }

    private void OnGUI()
    {
        DrawAddPathButton();
        DrawCancelButton();
    }

    private void DrawAddPathButton()
    {
        if(GUILayout.Button("Create New Path"))
        {
            PathCreationController.StartPathCreation();
            Debug.Log("path creation started, button pressed");
        }
    }

    private void DrawCancelButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            PathCreationController.EndPathCreation();
            Debug.Log("path creation cancelled, button pressed");
        }
    }
}
