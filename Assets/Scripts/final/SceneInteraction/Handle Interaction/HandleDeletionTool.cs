using UnityEditor;
using UnityEngine;

public class HandleDeletionTool : ISceneInteractionTool
{
    GameObject hoveredObject;

    public void StartInteraction()
    {
        RuntimeInteractionData.isDeletingHandle = true;
        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, onMiss);
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.isDeletingHandle = false;
        SceneRaycastListener.StopRaycastListener();
    }

    public void OnHover(RaycastHit hitInfo)
    {
        hoveredObject = hitInfo.collider.gameObject;

        if (hoveredObject.GetComponent<HandleOnPathComponent>())
        {
            Bounds bounds = hoveredObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }
    }

    public bool OnLeftClick(RaycastHit hitInfo)
    {
        bool useLeftClick = false;
        GameObject hitObject = hitInfo.collider.gameObject;
        HandleOnPathComponent handleComponent = hitObject.GetComponent<HandleOnPathComponent>();

        if (handleComponent)
    {
            Handle handle = handleComponent.Handle;
            Path path = handleComponent.Path;
            useLeftClick = true;
            HandleManager.Instance.DeleteHandleOnPath(handle, path);
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
