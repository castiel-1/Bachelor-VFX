using Unity.VisualScripting;
using UnityEngine;

public class PathDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;

    private LineRenderer lineRenderer;

    public void DisplayPoints(Vector3[] points)
    {
        foreach (Vector3 point in points)
        {
            Instantiate(pointPrefab, point, Quaternion.identity);
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
}
