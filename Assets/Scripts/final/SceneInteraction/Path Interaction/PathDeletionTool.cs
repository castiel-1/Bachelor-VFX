using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PathDeletionTool : ISceneInteractionTool
{
    private GameObject hoveredObject;

    public void StartInteraction()
    {
        GraphOperations.TogglePathPoints(true);
        RuntimeInteractionData.isDeletingPath = true;
        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, OnMiss);
    }

    public void StopInteraction()
    {
        GraphOperations.TogglePathPoints(false);
        RuntimeInteractionData.isDeletingPath = false;
        hoveredObject = null;
        SceneRaycastListener.StopRaycastListener();
    }

    public void OnHover(RaycastHit hitInfo)
    {

        hoveredObject = hitInfo.collider.gameObject;

        if (hoveredObject.GetComponent<PathPointComponent>())
        {
            Path path = hoveredObject.GetComponent<PathPointComponent>().Path;
            Graph graph = hoveredObject.GetComponent<PathPointComponent>().Graph;
            GraphDisplayer graphDisplayer = graph.GetComponent<GraphDisplayer>();
            List<GameObject> pathPoints = graphDisplayer.pathPointObjects[path];

            Handles.color = UnityEngine.Color.red;

            foreach (GameObject point in pathPoints)
            {
                Bounds bounds = point.GetComponent<Collider>().bounds;
                Handles.DrawWireCube(bounds.center, bounds.size);
            }
        }

    }

    public bool OnLeftClick(RaycastHit hitInfo)
    {
        bool useLeftClick;

        GameObject hitObject = hitInfo.collider.gameObject;
        PathPointComponent pathPointComp = hitObject.GetComponent<PathPointComponent>();
        Path path = pathPointComp.Path;
        Graph graph = pathPointComp.Graph;

        if (pathPointComp)
        {
            GraphOperations.DeletePath(graph, path);
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
