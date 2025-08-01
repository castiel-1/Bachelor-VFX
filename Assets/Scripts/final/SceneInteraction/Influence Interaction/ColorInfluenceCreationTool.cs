using UnityEngine;

public class ColorInfluenceCreationTool : ISceneInteractionTool
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

        InfluenceManager.Instance.AddColorInfluence
            (
                RuntimeInteractionData.influenceName,
                position,
                RuntimeInteractionData.influenceRadius,
                RuntimeInteractionData.influenceObject,
                RuntimeInteractionData.influenceColor
            );

        StopInteraction();
    }
}
