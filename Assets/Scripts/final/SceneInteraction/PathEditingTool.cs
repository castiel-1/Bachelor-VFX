using UnityEngine;

public class PathEditingTool : ISceneInteractionTool
{
    public void StartInteraction()
    {
        RuntimeInteractionData.IsEditingPath = true;
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.IsEditingPath = false;
    }

    
}
