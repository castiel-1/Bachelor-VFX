using Obi;
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
    private HandleCreationTool handleCreationTool = new();

    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<GraphInteraction>("Graph Interaction");
    }

    private void OnGUI()
    {
        // path creation
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Creation", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            DrawPathCreationDropdown();
            DrawAddPathButton();

            if (RuntimeInteractionData.AskForCursorPositionConfirmation)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);
                    DrawCursorPositionConfirmationButton();
                }
                EditorGUILayout.EndHorizontal();
            }

            if (RuntimeInteractionData.IsCreatingPath)
            {
                DrawCancelPathCreationButton();
            }
        }
        EditorGUILayout.EndVertical();

        // path deletion
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Deletion", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            DrawDeletePathButton();

            if (RuntimeInteractionData.IsDeletingPath)
            {
                DrawCancelPathDeletionButton();
            }
        }
        EditorGUILayout.EndVertical();

        // path editing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Editing", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            DrawPathEditingButton();

            if (RuntimeInteractionData.IsEditingPath)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);
                    DrawAddHandleButton();
                }
                EditorGUILayout.EndHorizontal();

                DrawCancelPathEditingButton();
            }
        }
        EditorGUILayout.EndVertical();
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

            RuntimeInteractionData.IsEditingPath = true;
        }
    }

    private void DrawAddHandleButton()
    {
        if (GUILayout.Button("Add Handle"))
        {
            // debugging
            Debug.Log("add handle button pressed");

            handleCreationTool.StartInteraction();
        }
    }

    private void DrawCancelPathEditingButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("cancel edit path button pressed");

            RuntimeInteractionData.IsEditingPath = false;
        }
    }

  
}
