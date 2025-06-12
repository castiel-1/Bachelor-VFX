using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GraphDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject nodePrefab;

    private Graph graph;

    private LineRenderer lineRenderer;
    private Dictionary<Path, List<GameObject>> pathObjects = new();
    private Dictionary<Node, GameObject> nodeObjects = new();

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
    }
    private void OnDisable()
    {
        graph.OnNodeCreated -= SpawnNode;
        graph.OnNodeDeleted -= DespawnNode;
        graph.OnPathCreated -= SpawnPath;
        graph.OnPathDeleted -= DespawnPath;
    }

    public void SpawnPath(Path path)
    {
        // debugging
        Debug.Log("path spawned");

        GameObject pathParent = new GameObject("Path_" + path.StartNode.ID + "_" + path.EndNode.ID);
        pathParent.transform.parent = transform;
        PathDestructionNotifier notifier = pathParent.AddComponent<PathDestructionNotifier>();
        notifier.LinkedPath = path;

        List<GameObject> pointObjects = new();

        foreach (Vector3 point in path.pathPoints)
        {
            GameObject nextPoint = Instantiate(pointPrefab, point, Quaternion.identity, pathParent.transform);
            pointObjects.Add(nextPoint);
        }

        pathObjects.Add(path, pointObjects);
    }

    public void DespawnPath(Path path)
    {
        // debugging
        Debug.Log("path despawned");

        foreach (GameObject pointGO in pathObjects[path])
        {
            Destroy(pointGO);
        }

        pathObjects.Remove(path);
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
    
    public void DespawnNode(Node node)
    {
        // debugging
        Debug.Log("node despawned");

        Destroy(nodeObjects[node]);

        nodeObjects.Remove(node);
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


}
