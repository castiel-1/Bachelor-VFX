using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{
    public Vector3[] controlPoints;
    public TextOnSpline textOnSpline;
    public GameObject prefab;
    public int numSamplePoints = 10;
    public bool drawLine;

    private GameObject[] prefabInstances;
    private LineRenderer lineRenderer;

    void Start()
    {
        SetUpControlPoints();
        SetUpLineRenderer();

        if (!drawLine)
        {
            lineRenderer.positionCount = 0;
        }

        Invoke(nameof(TextAndLineOneFrameDelayed), 0);

    }

    // have to delay updating the vfx graph otherwise it won't be set up when we try to update it the first time
    private void TextAndLineOneFrameDelayed()
    {

        Vector3[] splinePoints = GenerateSpline(controlPoints);

        if (drawLine)
        {
            DrawSpline(splinePoints);
        }
        textOnSpline.OnSplineUpdated(splinePoints);
    }

    void Update()
    {
        if (SplineHasChanged())
        {
            Vector3[] splinePoints = GenerateSpline(controlPoints);

            if (drawLine)
            {
                DrawSpline(splinePoints);
            }
            textOnSpline.OnSplineUpdated(splinePoints);
        }
    }
    public bool SplineHasChanged()
    {
        bool hasChanged = false;

        for(int i = 0; i < prefabInstances.Length; i++)
        {
            if(prefabInstances[i].transform.position != controlPoints[i])
            {
                controlPoints[i] = prefabInstances[i].transform.position;
                hasChanged = true;
            }
            
        }

        return hasChanged;
    }
    public void SetUpControlPoints()
    {
        prefabInstances = new GameObject[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            prefabInstances[i] = Instantiate(prefab, controlPoints[i], Quaternion.identity);
        }
    }

    public void SetUpLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    public void DrawSpline(Vector3[] splinePoints)
    {
        lineRenderer.positionCount = splinePoints.Length;
        lineRenderer.SetPositions(splinePoints);
    }

    public Vector3[] GenerateSpline(Vector3[] controlPoints)
    {
        int splineSegmentCount = controlPoints.Length - 3;
        int splinePointCount = splineSegmentCount * numSamplePoints;
        Vector3[] splinePoints = new Vector3[splinePointCount];

        int index = 0;

        for (int i = 0; i < controlPoints.Length - 3; i++)
        {
            Vector3 p0 = controlPoints[i];
            Vector3 p1 = controlPoints[i + 1];
            Vector3 p2 = controlPoints[i + 2];
            Vector3 p3 = controlPoints[i + 3];


            for (int j = 0; j < numSamplePoints; j++)
            {
                float t = j / (float)(numSamplePoints - 1);
                splinePoints[index] = GetCatmullRomPoint(p0, p1, p2, p3, t);
                index++;
            }
        }

        return splinePoints;
    }

    private Vector3 GetCatmullRomPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 calculatedPoint =
        0.5f * 
        ((2 * p1) +
        (-p0 + p2) * t +
        (2 * p0 - 5 * p1 + 4 * p2 - p3) * (t * t) +
        (-p0 + 3 * p1 - 3 * p2 + p3) * (t * t * t)
        );

        return calculatedPoint;
    }

    
}
