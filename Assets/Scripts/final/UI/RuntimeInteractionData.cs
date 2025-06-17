using UnityEngine;

public static class RuntimeInteractionData
{
    public enum PathCreationType { NodeToCursor, NodeToNode };

    public static PathCreationType pathCreationType;

    public static bool IsCreatingPath = false;

    public static bool AskForCursorPositionConfirmation = false;

    public static bool IsDeletingPath = false;

    public static bool IsEditingPath = false;
}
