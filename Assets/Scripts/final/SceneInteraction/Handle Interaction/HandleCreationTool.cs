using System.Drawing;
using UnityEditor;
using UnityEngine;

public class HandleCreationTool : ISceneInteractionTool
{
    private GameObject hoveredObject;

    public void StartInteraction()
    {
        RuntimeInteractionData.isCreatingHandle = true;
        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, OnMiss);
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.isCreatingHandle = false;
        SceneRaycastListener.StopRaycastListener();
    }

    public void OnHover(RaycastHit hitInfo)
    {
        hoveredObject = hitInfo.collider.gameObject;

        if(hoveredObject.GetComponent<PathPointComponent>())
        {
            Bounds bounds = hoveredObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }
    }

    public bool OnLeftClick(RaycastHit hitInfo)
    {
        bool useLeftClick;
        GameObject hitObject = hitInfo.collider.gameObject;
        Vector3 position = hitObject.transform.position;
        PathPointComponent pathPointComp = hitObject.GetComponent<PathPointComponent>();

        if (pathPointComp)
        {
            Path path = pathPointComp.Path;
            int pathPointIndex = pathPointComp.PointIndex;
            HandleManager.Instance.CreateHandleOnPath(position, path, pathPointIndex);

            useLeftClick = true;
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
}
