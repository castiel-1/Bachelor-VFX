using UnityEditor;
using UnityEngine;

public class GraphInteraction : EditorWindow
{
    // dropdowns
    private string[] pathCreationOptionsNotOnSurface = new string[] { "Node to Cursor", "Node to Node"};
    private string[] pathCreationOptionsOnSurface = new string[] { "Node to Surface Point", "Node to Node" };

    // tools
    private PathCreationTool pathCreationTool = new();
    private PathDeletionTool pathDeletionTool = new();
    private PathEditingTool pathEditingTool = new();

    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<GraphInteraction>("Graph Interaction");
    }

    private void OnGUI()
    {
        // path creation section
        DrawPathCreationDropdown();
        DrawAddPathButton();

        if (RuntimeInteractionData.AskForCursorPositionConfirmation)
        {
            DrawCursorPositionConfirmationButton();
        }

        if (RuntimeInteractionData.IsCreatingPath)
        {
            DrawCancelPathCreationButton();
        }

        // path deletion section
        DrawDeletePathButton();

        if (RuntimeInteractionData.IsDeletingPath)
        {
            DrawCancelPathDeletionButton();
        }

        // path editing
        DrawPathEditingButton();

        if (RuntimeInteractionData.IsEditingPath)
        {
            DrawAddControlPointButton();
            DrawCancelPathEditingButton();
        }
    }

    private void DrawPathCreationDropdown()
    {
        if (RuntimeSettingsData.onSurface)
        {
            RuntimeInteractionData.pathCreationType = 
                (RuntimeInteractionData.PathCreationType)EditorGUILayout.Popup("Path Creation Type", (int)RuntimeInteractionData.pathCreationType, pathCreationOptionsOnSurface);
        }
        else
        {
            RuntimeInteractionData.pathCreationType = 
                (RuntimeInteractionData.PathCreationType)EditorGUILayout.Popup("Path Creation Type", (int)RuntimeInteractionData.pathCreationType, pathCreationOptionsNotOnSurface);

        }
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

    private void DrawCancelPathCreationButton()
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
            // debugging
            Debug.Log("path deletion started");

            pathDeletionTool.StartInteraction();
        }
    }

    private void DrawCancelPathDeletionButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("path deletion ended");

            pathDeletionTool.StopInteraction();
        }
    }

    private void DrawPathEditingButton()
    {
        if(GUILayout.Button("Edit Path"))
        {
            // debugging
            Debug.Log("path editing started");

            pathEditingTool.StartInteraction();
        }
    }

    private void DrawAddControlPointButton()
    {
        if(GUILayout.Button("Add Control Point"))
        {
            // debugging
            Debug.Log("control point button pressed");


        }
    }

    private void DrawCancelPathEditingButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("cancel edit path button pressed");

            pathEditingTool.StopInteraction();
        }
    }
}
