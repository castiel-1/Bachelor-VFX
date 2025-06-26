using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GraphDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject nodePrefab;
    public GameObject handlePrefab;

    private Graph graph;

    private LineRenderer lineRenderer;
    private Dictionary<Path, List<GameObject>> pathPointObjects = new();

    public Dictionary<Path, List<GameObject>> PathObjects => pathPointObjects; //  make it accessible but read only

    private Dictionary<Path, GameObject> pathObjects = new();
    private Dictionary<Node, GameObject> nodeObjects = new();
    private Dictionary<Handle, GameObject> handleOnNodeObjects = new();

    private void Awake()
    {
        graph = GetComponent<Graph>();
    }

    private void OnEnable()
    {
        graph.OnNodeCreated += SpawnNode;
        graph.OnNodeDeleted += DespawnNode;
        graph.OnPathCreated += SpawnPath;
        graph.OnPathDeleted += DespawnPath;

        HandleOperations.OnHandleOnNodeCreated += SpawnHandleOnNode;
        HandleOperations.OnHandleOnNodeDestroyed += DespawnHandleOnNode;
        HandleOperations.OnSplineUpdated += UpdatePathPoints;
        HandleOperations.OnToggleAllHandles += ToggleHandles;
    }
    private void OnDisable()
    {
        graph.OnNodeCreated -= SpawnNode;
        graph.OnNodeDeleted -= DespawnNode;
        graph.OnPathCreated -= SpawnPath;
        graph.OnPathDeleted -= DespawnPath;

        HandleOperations.OnHandleOnNodeDestroyed -= DespawnHandleOnNode;
        HandleOperations.OnHandleOnNodeCreated -= SpawnHandleOnNode;
        HandleOperations.OnSplineUpdated -= UpdatePathPoints;
        HandleOperations.OnToggleAllHandles -= ToggleHandles;
    }

    public void SpawnPath(Path path)
    {
        // debugging
        Debug.Log("path spawned");

        GameObject pathParent = new GameObject("Path_" + path.StartNode.ID + "_" + path.EndNode.ID);
        pathParent.transform.parent = transform;

        pathObjects[path] = pathParent;

        List<GameObject> pointObjects = new();

        for(int i = 0; i < path.pathPoints.Length; i++)
        {
            GameObject nextPoint = Instantiate(pointPrefab, path.pathPoints[i], Quaternion.identity, pathParent.transform);
            nextPoint.AddComponent<PathPointComponent>().Initialize(path, graph, i);

            pointObjects.Add(nextPoint);
        }

        pathPointObjects.Add(path, pointObjects);
    }

    public void DespawnPath(Path path)
    {
        // debugging
        Debug.Log("path despawned");

        // destroy path point gameobjects
        foreach (GameObject pointGO in pathPointObjects[path])
        {
            Destroy(pointGO);
        }

        // destroy path gameobject
        Destroy(pathObjects[path]);

        // remove from the dictionaries
        pathObjects.Remove(path);
        pathPointObjects.Remove(path);
    }

    public void SpawnHandleOnNode(Handle handle, Node node)
    {
        // debugging
        Debug.Log("spawn handle on node called");

        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity, transform);
        handleGO.name = "nodeHandle_" + node.ID;
        var handleComponent = handleGO.AddComponent<HandleOnNodeComponent>();
        handleComponent.Initialize(node, handle);

        handleGO.SetActive(false);

        handleOnNodeObjects[handle] = handleGO;
    }

    public void SpawnNode(Node node, Graph graph)
    {
        // debugging
        Debug.Log("node spawned");

        GameObject nodeGO = Instantiate(nodePrefab, node.Position, Quaternion.identity, transform);
        nodeGO.name = "Node_" + node.ID;
        nodeGO.AddComponent<NodeComponent>().Initialize(node, graph);

        nodeObjects.Add(node, nodeGO);
    }

    public void DespawnNode(Node node, Path path)
    {
        // debugging
        Debug.Log("node despawned");

        Destroy(nodeObjects[node]);

        nodeObjects.Remove(node);

        HandleOperations.DeleteHandleOnNode(node, path);
    }

    public void DespawnHandleOnNode(Handle handle)
    {
        GameObject handleGO = handleOnNodeObjects[handle];

        Destroy(handleGO);
        handleOnNodeObjects.Remove(handle);
    }

    public void UpdatePathPoints(Path path)
    {
        // debugging
        Debug.Log("updating path Points");
        Debug.Log($"Before OnSplineUpdated: path.pathPoints first = {path.pathPoints[0]}, last = {path.pathPoints[^1]}");

        List<GameObject> pathPoints = pathPointObjects[path];

        for(int i = 0;  i < pathPoints.Count; i++)
        {
            pathPoints[i].transform.position = path.pathPoints[i];
        }
    }

    public void DisplayLines(Vector3[] points)
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.useWorldSpace = true;

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    public void ToggleHandles(bool active)
    {
        if (active)
        {
            foreach (GameObject handle in handleOnNodeObjects.Values)
            {
                handle.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject handle in handleOnNodeObjects.Values)
            {
                handle.SetActive(false);
            }
        }
    }
}
