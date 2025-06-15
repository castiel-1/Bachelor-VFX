using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PathDeletionTool : ISceneInteractionTool
{
    private GameObject hoveredObject;

    public void StartInteraction()
    {
        RuntimeInteractionData.IsDeletingPath = true;
        SceneRaycastListener.StartRaycastListener(OnHover, OnLeftClick, OnMiss);
    }

    public void StopInteraction()
    {
        RuntimeInteractionData.IsDeletingPath = false;
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
            List<GameObject> pathPoints = graphDisplayer.PathObjects[path];

            Handles.color = UnityEngine.Color.red;

            foreach (GameObject point in pathPoints)
            {
                Bounds bounds = point.GetComponent<Collider>().bounds;
                Handles.DrawWireCube(bounds.center, bounds.size);
            }
        }
        else
        {
            Handles.color = UnityEngine.Color.red;
            Bounds bounds = hoveredObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }

    }

    public bool OnLeftClick(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.gameObject;
        Path path = hitObject.GetComponent<PathPointComponent>().Path;
        Graph graph = hitObject.GetComponent<PathPointComponent>().Graph;

        GraphOperations.DeletePath(graph, path);

        return true;
    }

    public void OnMiss()
    {
        hoveredObject = null;
    }
}
