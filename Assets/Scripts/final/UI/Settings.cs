using UnityEngine;
using UnityEditor;

public class Settings : EditorWindow
{
    // foldouts
    private bool showPathSettings = false;
    private bool showTextSettings = false;

    // dropdowns
    private string[] pathTypeOptions = new string[] { "Bezier", "Path On Mesh" };

    private string[] pathCreationModeOptions = new string[] { "Manual", "Auto" };

    private string[] textSizeOptions = new string[] { "One Size", "Random", "Growing", "Shrinking", "Wave" };

    [MenuItem("Window/Prototype Settings")]
    public static void ShowWindow()
    {
        GetWindow<Settings>("Prototype Settings");
    }

    private void OnGUI()
    {
        DrawPathSettings();
        DrawTextSettings();
    }

    private void DrawPathSettings()
    {
        // foldout
        showPathSettings = EditorGUILayout.Foldout(showPathSettings, "Path Settings", true);

        if (!showPathSettings)
        {
            return;
        }

        EditorGUI.indentLevel++;

        DrawPathTypeDropdown();
        DrawCreationModeDropdown();
        DrawGravityToggle();

        EditorGUI.indentLevel--;
    }
    
    // path type dropdown
    private void DrawPathTypeDropdown()
    {
        RuntimeSettings.pathType = (RuntimeSettings.PathType) (EditorGUILayout.Popup("Path Type", (int) RuntimeSettings.pathType, pathTypeOptions));
    }

    // creation mode dropdown
    private void DrawCreationModeDropdown()
    {
        RuntimeSettings.creationMode = (RuntimeSettings.CreationMode) EditorGUILayout.Popup("Creation Mode", (int) RuntimeSettings.creationMode, pathCreationModeOptions);
    }

    // gravity toggle
    private void DrawGravityToggle()
    {
        RuntimeSettings.gravity = EditorGUILayout.Toggle("Gravity", RuntimeSettings.gravity);
    }

    private void DrawTextSettings()
    {
        showTextSettings = EditorGUILayout.Foldout(showTextSettings, "Text Settings", true);

        if(!showTextSettings)
        {
            return;
        }

        EditorGUI.indentLevel++;

        DrawTextSizeDropdown();
        DrawMinMaxNumberOfLettersField();

        EditorGUI.indentLevel--;

    }

    private void DrawTextSizeDropdown()
    {
        RuntimeSettings.textSizeMode = (RuntimeSettings.TextSizeMode) EditorGUILayout.Popup("Text Size Mode", (int) RuntimeSettings.textSizeMode, textSizeOptions);

        if((int) RuntimeSettings.textSizeMode == 0)
        {
            EditorGUI.indentLevel++;

            DrawOneSizeField();

            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;

            DrawMinMaxSizeField();
            
            EditorGUI.indentLevel-- ;
        }
    }

    private void DrawOneSizeField()
    {
        RuntimeSettings.textSize = EditorGUILayout.FloatField("Text Size", RuntimeSettings.textSize);
    }

    private void DrawMinMaxSizeField()
    {
        RuntimeSettings.textSizeMin = EditorGUILayout.FloatField("Text Size Minimum", RuntimeSettings.textSizeMin);
        RuntimeSettings.textSizeMax = EditorGUILayout.FloatField("Text Size Maximum", RuntimeSettings.textSizeMax);
    }

    private void DrawMinMaxNumberOfLettersField()
    {
        EditorGUILayout.LabelField("Number of Letters");

        EditorGUI.indentLevel++;

        RuntimeSettings.numberOfLettersMin = EditorGUILayout.IntField("Minimun", RuntimeSettings.numberOfLettersMin);
        RuntimeSettings.numberOfLettersMax = EditorGUILayout.IntField("Maximum", RuntimeSettings.numberOfLettersMax);

        EditorGUI.indentLevel--;
    }
}
