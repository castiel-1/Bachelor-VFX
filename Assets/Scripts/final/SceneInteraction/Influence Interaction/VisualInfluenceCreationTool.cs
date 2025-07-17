using UnityEngine;

public class VisualInfluenceCreationTool : ISceneInteractionTool
{
    public void StartInteraction()
    {
        RuntimeInteractionData.isCreatingVisualInfluence = true;
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.isCreatingVisualInfluence = false;
    }

    public void HandleInfluenceCreationConfirmation()
    {
        Vector3 position = TargetCursor.Instance.GetCursorPosition();

        InfluenceManager.Instance.AddVisualInfluence
            (
                RuntimeInteractionData.influenceName,
                position,
                RuntimeInteractionData.influenceRadius,
                RuntimeInteractionData.influenceObject,
                RuntimeInteractionData.influenceColor
            );
    }
}
