using UnityEngine;

public static class RuntimeInteractionData
{
    public enum PathCreationType { NodeToCursor, NodeToNode };

    public static PathCreationType pathCreationType;

    public static bool isCreatingGraph = false;

    public static bool isCreatingPath = false;

    public static bool askForPathCreationCursorPositionConfirmation = false;

    public static bool isDeletingPath = false;

    public static bool isEditingPath = false;

    public static bool isCreatingHandle = false;

    public static bool isDeletingHandle = false;

    // general influence creation
    public static string influenceName = "";

    public static float influenceRadius = 0.8f;

    public static GameObject influenceObject = null;

    // semantic influence creation
    public static bool isCreatingSemanticInfluence = false;

    public static string influenceModifier = "";


    // visual influence creation
    public static bool isCreatingVisualInfluence = false;

    public static Color influenceColor = Color.white;

    // influence visibility toggle
    public static bool influenceVisible = true;

    public static bool influenceRadiusVisible = true;

    // orientation
    public static bool isEditingTextOrientation = false;

    public static Vector3 orientation = Vector3.up;
}
