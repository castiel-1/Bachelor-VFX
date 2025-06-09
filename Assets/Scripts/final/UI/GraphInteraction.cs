using UnityEditor;
using UnityEngine;

public class GraphInteraction : EditorWindow
{
    // dropdowns
    private int selectedPathCreationIndex = 0;
    private string[] pathCreationOptions = new string[] { "Node to Node", "Node to Cursor" };


    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<GraphInteraction>("Graph Interaction");
    }

    private void OnGUI()
    {
        DrawPathCreationDropdown();
        DrawAddPathButton();
        DrawCancelButton();
    }

    private void DrawPathCreationDropdown()
    {
        selectedPathCreationIndex = EditorGUILayout.Popup("Path Creation Type", selectedPathCreationIndex, pathCreationOptions);
    }

    private void DrawAddPathButton()
    {
        if(GUILayout.Button("Create New Path"))
        {
            PathCreationController.StartPathCreation(GetSelectedPathCreationStrategy());
            Debug.Log("path creation started, button pressed");
        }
    }
    private IPathCreationStrategy GetSelectedPathCreationStrategy()
    {
        IPathCreationStrategy strategy = null;

        switch(selectedPathCreationIndex)
        {
            case 0: strategy = new NodeToNodePathCreationStrategy(); break;
            case 1: strategy = new NodeToCursorPathCreationStrategy(); break;
        }

        return strategy;
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
