using UnityEditor;
using UnityEngine;

public class HandleDeletionTool : ISceneInteractionTool
{
    GameObject hoveredObject;

    public void StartInteraction()
    {
        RuntimeInteractionData.IsDeletingHandle = true;
        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, onMiss);
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.IsDeletingHandle = false;
        SceneRaycastListener.StopRaycastListener();
    }

    public void OnHover(RaycastHit hitInfo)
    {
        hoveredObject = hitInfo.collider.gameObject;

        if (hoveredObject.GetComponent<HandleComponent>())
        {
            Bounds bounds = hoveredObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }
    }

    public bool OnLeftClick(RaycastHit hitInfo)
    {
        bool useLeftClick = false;
        GameObject hitObject = hitInfo.collider.gameObject;
        HandleComponent handleComponent = hitObject.GetComponent<HandleComponent>();
        Handle handle = handleComponent.Handle;
        Path path = handleComponent.Path;

        if (handleComponent)
        {
            useLeftClick = true;
            HandleOperations.DeleteHandle(handle, path);
        }
        else
        {
            useLeftClick = false;
        }

        return useLeftClick;
    }

    public void onMiss()
    {
        hoveredObject = null;
    }


}
