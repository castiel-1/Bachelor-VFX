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
    private HandleDeletionTool handleDeletionTool = new();

    // handle displayer
    private PathHandleDisplayer handleDisplayer;

 

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

            // add path button
            DrawCreatePathButton();

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
                // cancel path creation button
                DrawCancelPathCreationButton();
            }
        }
        EditorGUILayout.EndVertical();

        // path deletion
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Deletion", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            // path deletion button
            DrawDeletePathButton();

            if (RuntimeInteractionData.IsDeletingPath)
            {
                // cancel path deletion button
                DrawCancelToolUseButton();
            }
        }
        EditorGUILayout.EndVertical();

        // path editing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Editing", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            // path editing button
            DrawPathEditingButton();

            if (RuntimeInteractionData.IsEditingPath)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);

                    // add handle button
                    DrawAddHandleButton();
                }
                EditorGUILayout.EndHorizontal();

                if (RuntimeInteractionData.IsCreatingHandle)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(40);

                        // cancel handle creation button
                        DrawCancelToolUseButton();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);

                    // handle deletion button
                    DrawDeleteHandleButton();
                }
                EditorGUILayout.EndHorizontal();

                if (RuntimeInteractionData.IsDeletingHandle)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(40);

                        // cancel handle deletion button
                        DrawCancelToolUseButton();
                    }
                    GUILayout.EndHorizontal();
                }

                // cancel path editing button
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

    private void DrawCreatePathButton()
    {
        if(GUILayout.Button("Create New Path"))
        {
            ToolManager.ActivateTool(pathCreationTool);
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
            ToolManager.DeactivateTool();

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

            ToolManager.ActivateTool(pathDeletionTool);
        }
    }

    private void DrawPathEditingButton()
    {
        if(GUILayout.Button("Edit Path"))
        {
            // debugging
            Debug.Log("path editing started");

            RuntimeInteractionData.IsEditingPath = true;

            // deactivate any active tool since we want to start editing the path so we don't want to have other tools active
            ToolManager.DeactivateTool();

            HandleOperations.ToggleAllHandles(true);
        }
    }
    private void DrawCancelPathEditingButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("cancel edit path button pressed");

            RuntimeInteractionData.IsEditingPath = false;

            ToolManager.DeactivateTool();

            HandleOperations.ToggleAllHandles(false);
        }
    }

    private void DrawAddHandleButton()
    {
        if (GUILayout.Button("Add Handle"))
        {
            // debugging
            Debug.Log("add handle button pressed");

            ToolManager.ActivateTool(handleCreationTool);
        }
    }

    private void DrawDeleteHandleButton()
    {
        if (GUILayout.Button("Delete Handle"))
        {
            // debugging
            Debug.Log("delete handle button pressed");

            ToolManager.ActivateTool(handleDeletionTool);
        }
    }

    private void DrawCancelToolUseButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("cancel tool use button pressed");

            ToolManager.DeactivateTool();
        }
    }

    private void DrawMoveHandleButton()
    {
        if(GUILayout.Button("Move Handle"))
        {
            // debugging
            Debug.Log("move handle button pressed");
        }
    }
}
