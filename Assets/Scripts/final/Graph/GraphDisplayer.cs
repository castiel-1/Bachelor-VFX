using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GraphDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject nodePrefab;

    private LineRenderer lineRenderer;
    private Dictionary<Node, GameObject> nodes = new();
    private Dictionary<Path, List<GameObject>> paths = new();

    private void OnEnable()
    {
        Graph.OnNodeCreated += SpawnNode;
        Graph.OnNodeDeleted += DespawnNode;
        Graph.OnPathCreated += SpawnPath;
        Graph.OnPathDeleted += DespawnPath;
    }
    private void OnDisable()
    {
        Graph.OnNodeCreated -= SpawnNode;
        Graph.OnNodeDeleted -= DespawnNode;
        Graph.OnPathCreated -= SpawnPath;
        Graph.OnPathDeleted -= SpawnPath;
    }

    public void SpawnPath(Path path)
    {
        // debugging
        Debug.Log("path spawned");

        GameObject pathParent = new GameObject();
        PathDestructionNotifier notifier = pathParent.AddComponent<PathDestructionNotifier>();
        notifier.LinkedPath = path;

        List<GameObject> pointObjects = new();

        foreach (Vector3 point in path.pathPoints)
        {
            GameObject nextPoint = Instantiate(pointPrefab, point, Quaternion.identity, pathParent.transform);
            pointObjects.Add(nextPoint);
        }

        paths.Add(path, pointObjects);
    }

    public void DespawnPath(Path path)
    {
        // debugging
        Debug.Log("path despawned");

        paths.Remove(path);
    }

    public void SpawnNode(Node node)
    {
        // debugging
        Debug.Log("node spawned");

        GameObject nextNode = Instantiate(nodePrefab, node.Position, Quaternion.identity);
        nodes.Add(node, nextNode);
    }
    
    public void DespawnNode(Node node)
    {
        // debugging
        Debug.Log("node despawned");

        Destroy(nodes[node]);

        nodes.Remove(node);
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
