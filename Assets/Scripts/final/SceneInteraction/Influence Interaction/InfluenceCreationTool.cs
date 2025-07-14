using UnityEngine;

public class InfluenceCreationTool : ISceneInteractionTool
{
    public void StartInteraction()
    {
        RuntimeInteractionData.isCreatingInfluence = true;
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.isCreatingInfluence = false;
    }

    public void HandleInfluenceCreationConfirmation()
    {
        Vector3 position = TargetCursor.Instance.GetCursorPosition();

        InfluenceManager.Instance.AddInfluence(
            position, RuntimeInteractionData.influenceName,
            RuntimeInteractionData.influenceModifier,
            RuntimeInteractionData.influenceRadius,
            RuntimeInteractionData.influenceObject);

        StopInteraction();
    }
}
