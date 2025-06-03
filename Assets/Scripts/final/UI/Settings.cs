using UnityEngine;
using UnityEditor;

public class Settings : EditorWindow
{
    // foldouts
    private bool showPathSettings = false;
    private bool showTextSettings = false;

    // dropdowns
    private int selectedPathTypeIndex = 0;
    private string[] pathTypeOptions = new string[] { "Bezier", "Path On Mesh" };

    private int selectedCreationModeIndex = 0;
    private string[] pathCreationModeOptions = new string[] { "Manual", "Auto" };

    private int selectedTextSizeIndex = 0;
    private string[] textSizeOptions = new string[] { "One Size", "Random", "Growing", "Shrinking", "Wave" };

    // toggles
    private bool gravity = true;

    // input fields
    private float textSize = 0.2f;
    private float textSizeMin = 0.1f;
    private float textSizeMax = 0.5f;

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
        selectedPathTypeIndex = EditorGUILayout.Popup("Path Type", selectedPathTypeIndex, pathTypeOptions);
    }

    // creation mode dropdown
    private void DrawCreationModeDropdown()
    {
        selectedCreationModeIndex = EditorGUILayout.Popup("Creation Mode", selectedCreationModeIndex, pathCreationModeOptions);
    }

    // gravity toggle
    private void DrawGravityToggle()
    {
        gravity = EditorGUILayout.Toggle("Gravity", gravity);
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

        EditorGUI.indentLevel--;
    }

    private void DrawTextSizeDropdown()
    {
        selectedTextSizeIndex = EditorGUILayout.Popup("Text Size Mode", selectedTextSizeIndex, textSizeOptions);

        if(selectedTextSizeIndex == 0)
        {
            EditorGUI.indentLevel++;

            DrawOneSizeField();

            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;

            DrawMixMaxSizeField();
            
            EditorGUI.indentLevel-- ;
        }
    }

    private void DrawOneSizeField()
    {
        textSize = EditorGUILayout.FloatField("Text Size", textSize);
    }

    private void DrawMixMaxSizeField()
    {
        textSizeMin = EditorGUILayout.FloatField("Text Size Minimum", textSizeMin);
        textSizeMax = EditorGUILayout.FloatField("Text Size Maximum", textSizeMax);
    }

}
