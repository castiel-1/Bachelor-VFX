using UnityEditor;
using UnityEngine;

public class GraphInteraction : EditorWindow
{
    // dropdowns
    private string[] pathCreationOptions = new string[] { "Node to Cursor", "Node to Node" };

    // tools
    private PathCreationTool pathCreationTool = new();


    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<GraphInteraction>("Graph Interaction");
    }

    private void OnGUI()
    {
        DrawPathCreationDropdown();
        DrawAddPathButton();

        if (RuntimeInteractionData.AskForCursorPositionConfirmation)
        {
            DrawCursorPositionConfirmationButton();
        }

        if (RuntimeInteractionData.IsCreatingPath)
        {
            DrawCancelButton();
        }

        DrawDeletePathButton();
    }

    private void DrawPathCreationDropdown()
    {
        RuntimeInteractionData.pathCreationType = (RuntimeInteractionData.PathCreationType) EditorGUILayout.Popup("Path Creation Type", (int) RuntimeInteractionData.pathCreationType, pathCreationOptions);
    }

    private void DrawAddPathButton()
    {
        if(GUILayout.Button("Create New Path"))
        {
            pathCreationTool.StartInteraction();
            Debug.Log("path creation started, button pressed");
        }
    }

    private void DrawCursorPositionConfirmationButton()
    {
        if(GUILayout.Button("Confirm Cursor Position"))
        {
            
            if(PathCreationTool.SelectedStrategy is NodeToCursorPathCreationStrategy nodeToCursorStrategy)
            {
                nodeToCursorStrategy.HandleCursorPositionConfirmation();
            }

        }
    }

    private void DrawCancelButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            pathCreationTool.StopInteraction();

            // hide cursor position confirmation button again
            RuntimeInteractionData.AskForCursorPositionConfirmation = false;

            Debug.Log("path creation cancelled, button pressed");
        }
    }

    private void DrawDeletePathButton()
    {
        if(GUILayout.Button("Delete Path"))
        {

        }
    }
}
