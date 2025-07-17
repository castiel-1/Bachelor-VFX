using UnityEngine;

public class SemanticInfluenceCreationTool : ISceneInteractionTool
{
    public void StartInteraction()
    {
        RuntimeInteractionData.isCreatingSemanticInfluence = true;
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.isCreatingSemanticInfluence = false;
    }

    public void HandleInfluenceCreationConfirmation()
    {
        Vector3 position = TargetCursor.Instance.GetCursorPosition();

        InfluenceManager.Instance.AddSemanticInfluence
            (
                RuntimeInteractionData.influenceName,
                position,
                RuntimeInteractionData.influenceRadius,
                RuntimeInteractionData.influenceObject,
                RuntimeInteractionData.influenceModifier
            );

        StopInteraction();
    }
}
