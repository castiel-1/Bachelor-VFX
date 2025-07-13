using UnityEngine;

public static class RuntimeInteractionData
{
    public enum PathCreationType { NodeToCursor, NodeToNode };

    public static PathCreationType pathCreationType;

    public static bool isCreatingGraph = false;

    public static bool IsCreatingPath = false;

    public static bool AskForPathCreationCursorPositionConfirmation = false;

    public static bool AskForGraphCreationCursorPositionConfirmation = false;

    public static bool IsDeletingPath = false;

    public static bool IsEditingPath = false;

    public static bool IsCreatingHandle = false;

    public static bool IsDeletingHandle = false;    
}
