using UnityEngine;

public static class ToolManager 
{
    private static ISceneInteractionTool currentTool;
    public static void ActivateTool(ISceneInteractionTool tool)
    {
        if(currentTool != null)
        {
            DeactivateTool();
        }

        currentTool = tool;

        currentTool.StartInteraction();
    }

    public static void DeactivateTool()
    {
        if(currentTool != null)
        {
            currentTool.StopInteraction();
            currentTool = null;
        }
    }
}
