using Obi;
using UnityEditor;
using UnityEngine;

public class InteractionUI : EditorWindow
{
    // dropdowns
    private string[] pathCreationOptionsNotOnSurface = new string[] { "Node to Cursor", "Node to Node"};
    private string[] pathCreationOptionsOnSurface = new string[] { "Node to Surface Point", "Node to Node" };

    // foldouts
    private bool showGraphinteractions = false;
    private bool showPathInteractions = false;
    private bool showInfluenceInteractions = false;

    // tools
    private GraphCreationTool graphCreationTool = new();
    private PathCreationTool pathCreationTool = new();
    private PathDeletionTool pathDeletionTool = new();
    private HandleCreationTool handleCreationTool = new();
    private HandleDeletionTool handleDeletionTool = new();
    private SemanticInfluenceCreationTool semanticInfluenceCreationTool = new();
    private VisualInfluenceCreationTool visualInfluenceCreationTool = new();

    [MenuItem("Window/Graph Interaction")]
    public static void ShowWindow()
    {
        GetWindow<InteractionUI>("Graph Interaction");
    }

    private void OnGUI()
    {
        // foldout graph
        showGraphinteractions = EditorGUILayout.Foldout(showGraphinteractions, "Graph Interaction", true);
        if (showGraphinteractions)
        {
            DrawGraphCreationUI();
        }

        // foldout path
        showPathInteractions = EditorGUILayout.Foldout(showPathInteractions, "Path Interactions", true);
        if (showPathInteractions)
        {
            DrawPathCreationUI();
            DrawPathDeletionUI();
            DrawPathEditingUI();
        }

        // foldout influence
        showInfluenceInteractions = EditorGUILayout.Foldout(showInfluenceInteractions, "Influence Interactions", true);
        if (showInfluenceInteractions)
        {
            DrawInfluenceCreationUI();
            DrawInfluenceVisibilityUI();
        }
    
    }
    private void DrawGraphCreationUI()
    {
        EditorGUILayout.LabelField("Graph Creation", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            DrawGraphCreationButton();

            if (RuntimeInteractionData.isCreatingGraph)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);
                    DrawGraphCreationCursorPositionConfirmationButton();
                }
                EditorGUILayout.EndHorizontal();

                DrawCancelGraphCreationButton();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawPathCreationUI()
    {
        // path creation
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Creation", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            DrawPathCreationDropdown();

            // add path button
            DrawCreatePathButton();

            if (RuntimeInteractionData.askForPathCreationCursorPositionConfirmation)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);
                    DrawPathCreationCursorPositionConfirmationButton();
                }
                EditorGUILayout.EndHorizontal();
            }

            if (RuntimeInteractionData.isCreatingPath)
            {
                // cancel path creation button
                DrawCancelPathCreationButton();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawPathDeletionUI()
    {
        // path deletion
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Deletion", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            // path deletion button
            DrawDeletePathButton();

            if (RuntimeInteractionData.isDeletingPath)
            {
                // cancel path deletion button
                DrawCancelToolUseButton();
            }
        }
        EditorGUILayout.EndVertical();
    }
    
    private void DrawPathEditingUI()
    {
        // path editing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Editing", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        {
            // path editing button
            DrawPathEditingButton();

            if (RuntimeInteractionData.isEditingPath)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(40);

                    // add handle button
                    DrawAddHandleButton();
                }
                EditorGUILayout.EndHorizontal();

                if (RuntimeInteractionData.isCreatingHandle)
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

                if (RuntimeInteractionData.isDeletingHandle)
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

    private void DrawInfluenceCreationUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Influence Creation", EditorStyles.boldLabel);

        // semantic influence creation
        EditorGUILayout.BeginVertical("box");
        {
            DrawCreateSemanticInfluenceButton();

            if (RuntimeInteractionData.isCreatingSemanticInfluence)
            {
                EditorGUI.indentLevel++;
                    DrawInfluenceNameField();
                    DrawInfluenceModifierField();
                    DrawInfluenceRadiusField();
                    DrawInfluenceGameObjectField();
                EditorGUI.indentLevel--;

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(15);
                    DrawSemanticInfluenceCreationConfirmationButton();
                }
                EditorGUILayout.EndHorizontal();

                DrawCancelToolUseButton();
            }
        }
        EditorGUILayout.EndVertical();

        // visual influence creation
        EditorGUILayout.BeginVertical("box");
        {
            DrawCreateVisualInfluenceButton();

            if (RuntimeInteractionData.isCreatingVisualInfluence)
            {
                EditorGUI.indentLevel++;
                    DrawInfluenceNameField();
                    DrawInfluenceColorField();
                    DrawInfluenceRadiusField();
                    DrawInfluenceGameObjectField();
                EditorGUI.indentLevel--;

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(15);
                    DrawVisualInfluenceCreationConfirmationButton();
                }
                EditorGUILayout.EndHorizontal();

                DrawCancelToolUseButton();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawInfluenceVisibilityUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Influence Visibility", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        {
            DrawInfluenceVisibilityToggle();

            if (RuntimeInteractionData.influenceVisible)
            {
                DrawInfluenceRadiusVisibilityToggle();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawGraphCreationButton()
    {
        if (GUILayout.Button("Create Graph"))
        {
            ToolManager.ActivateTool(graphCreationTool);
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

    private void DrawCreatePathButton()
    {
        if(GUILayout.Button("Create New Path"))
        {
            ToolManager.ActivateTool(pathCreationTool);
            Debug.Log("path creation started, button pressed");
        }
    }

    private void DrawGraphCreationCursorPositionConfirmationButton()
    {
        if(GUILayout.Button("Confirm Cursor Position"))
        {
            graphCreationTool.HandleCursorConfirmation();
        }
    }

    private void DrawPathCreationCursorPositionConfirmationButton()
    {
        if(GUILayout.Button("Confirm Cursor Position"))
        {
            
            if(PathCreationTool.SelectedStrategy is NodeToCursorPathCreationStrategy nodeToCursorStrategy)
            {
                nodeToCursorStrategy.HandleCursorPositionConfirmation();
            }

        }
    }

    private void DrawCancelGraphCreationButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            ToolManager.DeactivateTool();
        }
    }

    private void DrawCancelPathCreationButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            ToolManager.DeactivateTool();

            // hide cursor position confirmation button again
            RuntimeInteractionData.askForPathCreationCursorPositionConfirmation = false;

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

            RuntimeInteractionData.isEditingPath = true;

            // deactivate any active tool since we want to start editing the path so we don't want to have other tools active
            ToolManager.DeactivateTool();

            HandleOperations.ToggleAllHandles(true);
            GraphOperations.TogglePathPoints(true);
        }
    }

    private void DrawCancelPathEditingButton()
    {
        if (GUILayout.Button("Cancel"))
        {
            // debugging
            Debug.Log("cancel edit path button pressed");

            RuntimeInteractionData.isEditingPath = false;

            ToolManager.DeactivateTool();

            HandleOperations.ToggleAllHandles(false);
            GraphOperations.TogglePathPoints(false);
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

    private void DrawCreateSemanticInfluenceButton()
    {
        if (GUILayout.Button("Create Semantic Influence"))
        {
            ToolManager.ActivateTool(semanticInfluenceCreationTool);
        }
    }

    private void DrawCreateVisualInfluenceButton()
    {
        if (GUILayout.Button("Create Visual Influence"))
        {
            ToolManager.ActivateTool(visualInfluenceCreationTool);
        }
    }

    private void DrawInfluenceNameField()
    {
        RuntimeInteractionData.influenceName = EditorGUILayout.TextField("Name", RuntimeInteractionData.influenceName);
    }

    private void DrawInfluenceRadiusField()
    {
        RuntimeInteractionData.influenceRadius = EditorGUILayout.FloatField("Radius", RuntimeInteractionData.influenceRadius);
    }

    private void DrawInfluenceGameObjectField()
    {
        RuntimeInteractionData.influenceObject = (GameObject)EditorGUILayout.ObjectField("Game Object", RuntimeInteractionData.influenceObject, typeof (GameObject), true);
    }

    private void DrawInfluenceModifierField()
    {
        EditorGUILayout.LabelField("Prompt Modifier");
        RuntimeInteractionData.influenceModifier = EditorGUILayout.TextArea(RuntimeInteractionData.influenceModifier, GUILayout.Height(60));
    }

    private void DrawSemanticInfluenceCreationConfirmationButton()
    {
        if(GUILayout.Button("Confirm Influence Parameters"))
        {
            semanticInfluenceCreationTool.HandleInfluenceCreationConfirmation();
        }
    }

    private void DrawVisualInfluenceCreationConfirmationButton()
    {
        if (GUILayout.Button("Confirm Visual Influence Parameters"))
        {
            visualInfluenceCreationTool.HandleInfluenceCreationConfirmation();
        }
    }

    private void DrawInfluenceColorField()
    {
        RuntimeInteractionData.influenceColor = EditorGUILayout.ColorField("Text Colour", RuntimeInteractionData.influenceColor);
    }

    private void DrawInfluenceVisibilityToggle()
    {

        bool oldValue = RuntimeInteractionData.influenceVisible;

        bool newValue = EditorGUILayout.Toggle("Show Influence", oldValue);

        if (newValue != oldValue)
        {
            RuntimeInteractionData.influenceVisible = newValue;
            InfluenceDisplayer.Instance.ToggleInfluenceVisibility(newValue);
        }
    }

    private void DrawInfluenceRadiusVisibilityToggle()
    {
        bool oldValue = RuntimeInteractionData.influenceRadiusVisible;

        bool newValue = EditorGUILayout.Toggle("Show Influence Radius", oldValue);

        if (newValue != oldValue)
        {
            RuntimeInteractionData.influenceRadiusVisible = newValue;
            InfluenceDisplayer.Instance.ToggleInfluenceRadiusVisibility(newValue);
        }
    }

}
