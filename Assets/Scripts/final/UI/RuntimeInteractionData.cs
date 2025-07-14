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

    public static bool isCreatingInfluence = false;

    public static string influenceName = "";

    public static string influenceModifier = "";

    public static float influenceRadius = 1f;

    public static GameObject influenceObject = null;
}
