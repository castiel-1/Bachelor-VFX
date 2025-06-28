using UnityEditor;
using UnityEngine;

public class PathCreationTool : ISceneInteractionTool
{
    private GameObject hoveredObject;

    private static IPathCreationStrategy selectedStrategy;
    public static IPathCreationStrategy SelectedStrategy => selectedStrategy;

    public void StartInteraction()
    {

        RuntimeInteractionData.AskForCursorPositionConfirmation = false;
        RuntimeInteractionData.IsCreatingPath = true;

        switch (RuntimeInteractionData.pathCreationType)
        {
            case RuntimeInteractionData.PathCreationType.NodeToNode: selectedStrategy = new NodeToNodePathCreationStrategy(this); break;
            case RuntimeInteractionData.PathCreationType.NodeToCursor: selectedStrategy = new NodeToCursorPathCreationStrategy(this); break;
        }

        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, OnMiss);
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.IsCreatingPath = false;
        SceneRaycastListener.StopRaycastListener();
        selectedStrategy = null;
        hoveredObject = null;
    }

    public void OnHover(RaycastHit hitInfo)
    {
        // don't do hover effect when we are waiting for cursor confirmation
        if (selectedStrategy is NodeToCursorPathCreationStrategy cursorStrategy && cursorStrategy.IsAwaitingCursorConfirmation)
        {
            hoveredObject = null;
            return;
        }

        hoveredObject = hitInfo.collider.gameObject;

        if (hoveredObject.GetComponent<NodeComponent>())
        {
            Handles.color = Color.red;
            Bounds bounds = hoveredObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }
    }

    // returns true if left click should be used (e.use), false if not
    public bool OnLeftClick(RaycastHit hitInfo)
    {
        // if we are waiting for the cursor position confirmation we stop using left click
        if (selectedStrategy is NodeToCursorPathCreationStrategy cursorStrategy && cursorStrategy.IsAwaitingCursorConfirmation)
        {
            return false;
        }

        bool useLeftClick = false;

        GameObject hitObject = hitInfo.collider.gameObject;
        NodeComponent nodeComponent = hitObject.GetComponent<NodeComponent>();

        if (nodeComponent)
        {
            useLeftClick = true;
            selectedStrategy.HandleClick(hitInfo);
        }
        else
        {
            useLeftClick = false;
        }

        return useLeftClick;
    }
    public void OnMiss()
    {
        hoveredObject = null;
    }

    private static int RandomizeNumberOfPathPoints(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }






}
