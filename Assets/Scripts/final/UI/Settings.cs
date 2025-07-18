using UnityEngine;
using UnityEditor;

public class Settings : EditorWindow
{
    // foldouts
    private bool showPathSettings = false;
    private bool showTextSettings = false;
    private bool showLLMSettings = false;

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
        DrawLLMSettings(); 
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
        RuntimeSettingsData.pathType = (RuntimeSettingsData.PathType) (EditorGUILayout.Popup("Path Type", (int) RuntimeSettingsData.pathType, pathTypeOptions));
    }

    // creation mode dropdown
    private void DrawCreationModeDropdown()
    {
        RuntimeSettingsData.creationMode = (RuntimeSettingsData.CreationMode) EditorGUILayout.Popup("Creation Mode", (int) RuntimeSettingsData.creationMode, pathCreationModeOptions);
    }

    // gravity toggle
    private void DrawGravityToggle()
    {
        RuntimeSettingsData.onSurface = EditorGUILayout.Toggle("On Surface", RuntimeSettingsData.onSurface);
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
        DrawMinMaxNumberOfWordsField();
        DrawColorPicker();

        EditorGUI.indentLevel--;

    }

    private void DrawTextSizeDropdown()
    {
        RuntimeSettingsData.textSizeMode = (RuntimeSettingsData.TextSizeMode) EditorGUILayout.Popup("Text Size Mode", (int) RuntimeSettingsData.textSizeMode, textSizeOptions);

        if((int) RuntimeSettingsData.textSizeMode == 0)
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
        RuntimeSettingsData.textSizeMin = EditorGUILayout.FloatField("Text Size", RuntimeSettingsData.textSizeMin);
    }

    private void DrawMinMaxSizeField()
    {
        RuntimeSettingsData.textSizeMin = EditorGUILayout.FloatField("Text Size Minimum", RuntimeSettingsData.textSizeMin);
        RuntimeSettingsData.textSizeMax = EditorGUILayout.FloatField("Text Size Maximum", RuntimeSettingsData.textSizeMax);
    }

    private void DrawMinMaxNumberOfWordsField()
    {
        EditorGUILayout.LabelField("Number of Words");

        EditorGUI.indentLevel++;

        RuntimeSettingsData.numberOfWordsMin = EditorGUILayout.IntField("Minimun", RuntimeSettingsData.numberOfWordsMin);
        RuntimeSettingsData.numberOfWordsMax = EditorGUILayout.IntField("Maximum", RuntimeSettingsData.numberOfWordsMax);

        EditorGUI.indentLevel--;
    }

    private void DrawColorPicker()
    {
        EditorGUILayout.LabelField("Text Colour Without Influence");

        EditorGUI.indentLevel++;
            RuntimeSettingsData.uninfluencedTextColor = EditorGUILayout.ColorField(GUIContent.none, RuntimeSettingsData.uninfluencedTextColor);
        EditorGUI.indentLevel--;
    }

    private void DrawLLMSettings()
    {
        // foldout
        showLLMSettings = EditorGUILayout.Foldout(showLLMSettings, "LLM Settings", true);

        if (!showLLMSettings)
        {
            return;
        }

        EditorGUI.indentLevel++;

        DrawHistoryDepthField();

        EditorGUI.indentLevel--;
    }

    private void DrawHistoryDepthField()
    {
        RuntimeSettingsData.historyDepth = EditorGUILayout.IntField("History Prompt Depth", RuntimeSettingsData.historyDepth);
    }
}

