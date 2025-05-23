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
        Graph.OnPathCreated += SpawnPath;
        Graph.OnPathDestroyed += DespawnPath;
    }

    public void SpawnPath(Path path)
    {
        List<GameObject> pointObjects = new();

        foreach (Vector3 point in path.pathPoints)
        {
            GameObject nextPoint = Instantiate(pointPrefab, point, Quaternion.identity);
            pointObjects.Add(nextPoint);
        }

        paths.Add(path, pointObjects);
    }

    public void DespawnPath(Path path)
    {
        foreach(GameObject point in paths[path])
        {
            Destroy(point);
        }

        paths.Remove(path);
    }

    public void SpawnNode(Node node)
    {
        GameObject nextNode = Instantiate(nodePrefab, node.Position, Quaternion.identity);
        nodes.Add(node, nextNode);
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
