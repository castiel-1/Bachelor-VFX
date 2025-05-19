using Unity.VisualScripting;
using UnityEngine;

public class PathDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;

    private LineRenderer lineRenderer;

    public void DisplayPathPoints(Vector3[] pathPoints)
    {
        foreach (Vector3 point in pathPoints)
        {
            Instantiate(pointPrefab, point, Quaternion.identity);
        }
    }

    public void DisplayPathLines(Vector3[] pathPoints)
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

        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }
}
