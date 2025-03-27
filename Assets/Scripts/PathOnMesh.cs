using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.VirtualTexturing;
using static PathOnMesh;

public class PathOnMesh : MonoBehaviour
{
    /* THE PLAN IS SIMPLE
    
      1) we pick a point on the mesh from the dictionary
      2) calculate normal vector with triangle corners
      3) calculate step vector
      4) make step
      5) if(intersection with next triangle)
            find intersection, remember how much of step is done, get corners of new triangle and go to 2)
         else 
            place point according to step, go to 4)

      */

    public Vector3 stepDirection;
    public float stepSize;
    public GraphicsInfoBuffer buffer;

   // private bool HadIntersection = false;
   // private (Vector3, Vector3) intersectionEdge = (Vector3.zero, Vector3.zero);

    public MeshManager meshManager;

    private Vector3[] path;
    private List<Vector3> debugPath = new List<Vector3>();
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();
    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();

    // Debugging
    private List<IntersectionInfo> intersectionInfos = new List<IntersectionInfo>();

    // Debugging
    public GameObject startPointP;
    public GameObject nextTheoreticalPointP;
    public GameObject nextPointP;

    // struct for intersection information
    public struct IntersectionInfo
    {
        // Debugging
        public Vector3 startPoint;
        public Vector3 nextTheoreticalPoint;


        public Vector3 nextPoint;
        public (Vector3, Vector3) edge;
        public Vector3 lastCorner;
        public bool hasIntersection;
        public float newStepSize;
    }

    void Start()
    {
        // create path
        CreatePath();
    }

    
    public void Visualize(int stepCount)
    {
        /*
        // draw start point red
        IntersectionInfo currentInfo = intersectionInfos[stepCount];
        Instantiate(startPointP, currentInfo.startPoint, quaternion.identity);

        // draw step vector blue
        GameObject lineObject = new GameObject("LineRendererObject");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;

        lineRenderer.SetPosition(0, currentInfo.startPoint);
        lineRenderer.SetPosition(1, currentInfo.nextTheoreticalPoint);

        if (currentInfo.hasIntersection)
        {
            // draw intersected edge green
            GameObject lineObject2 = new GameObject("LineRendererObject");
            LineRenderer lineRenderer2 = lineObject2.AddComponent<LineRenderer>();

            lineRenderer2.startWidth = 0.005f;
            lineRenderer2.endWidth = 0.005f;
            lineRenderer2.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer2.positionCount = 2;
            lineRenderer2.startColor = Color.green;
            lineRenderer2.endColor = Color.green;

            lineRenderer2.SetPosition(0, currentInfo.edge.Item1);
            lineRenderer2.SetPosition(1, currentInfo.edge.Item2);

        }

        // draw next point orange
        Instantiate(nextPointP, currentInfo.nextPoint, quaternion.identity);

        */
    }
    
    // Debugging display of path
    private void OnDrawGizmos()
    {
        /*
        float r = 0.01f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A, r);
        Gizmos.DrawSphere(B, r);
        Gizmos.DrawSphere(C, r);
        Gizmos.DrawSphere(D, r);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(C, D);
        */

        
        // display points
        Gizmos.color = Color.red;
        foreach (Vector3 point in path)
        {
            Gizmos.DrawSphere(point, 0.008f);
        }

        Gizmos.color = Color.blue;
        // display all points
        foreach(Vector3 point in debugPath)
        {
            Gizmos.DrawSphere(point, 0.008f);
        }

        // draw lines between points
        Gizmos.color = Color.green;
        for (int i = 0; i < path.Length - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }

        for (int i = 0; i < debugPath.Count - 1; i++)
        {
            Gizmos.DrawLine(debugPath[i], debugPath[i + 1]);
        }


        /*
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPoint, 0.02f);
        */
        
    }


    // TODO

    // calculate one path
    public void CreatePath()
    {

        // set up path array
        int numPoints = buffer.text.Length;
        path = new Vector3[numPoints];

        // get triangleDictionary
        triangleDict = meshManager.GetNeighbouringTrianglesDict();

        // get sortedTrianglesDcitionary
        sortedTrianglesDict = meshManager.GetSortedTrianglesDict();

        // get all the variables we need to make the initial step
        var corners = GetRandomTriangleOnMesh();
   
        // Debugging
        Debug.Log("random corners chosen: " + corners.Item1 + ", " + corners.Item2 + ", " +  corners.Item3);

        Vector3 startPoint = getStartPoint(corners.Item1, corners.Item2, corners.Item3);
        Vector3 normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);
        Vector3 step = GetStepVector(stepDirection, normal, stepSize, startPoint); 
        Vector3 nextTheoreticalPoint = startPoint + step;

        /*
        // Debugging
        Instantiate(vertex, corners.Item1, quaternion.identity);
        Instantiate(vertex, corners.Item2, quaternion.identity);
        Instantiate(vertex, corners.Item3, quaternion.identity);
        */

        // set up previousEdge
        (Vector3, Vector3)? previousEdge = null;

        // add startPoint to path
        path[0] = startPoint;

        // for as many letters as we want to display (starting at 2 because of startPoint)
        for (int i = 1; i < numPoints; i++)
        {
            // Debugging
            Debug.Log("previous edge: " + previousEdge);

            // calculate next point (either on the triangle or on an edge)
            IntersectionInfo info = NextStepInfo(corners.Item1, corners.Item2, corners.Item3, startPoint, nextTheoreticalPoint, previousEdge);

            // Debugging
            intersectionInfos.Add(info);

            /*
            // Debugging
            if(i == 1)
            {
                GameObject lineObject = new GameObject("LineRendererObject");
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

                lineRenderer.startWidth = 0.005f;
                lineRenderer.endWidth = 0.005f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.positionCount = 2;

                lineRenderer.SetPosition(0, info.edge.Item1);
                lineRenderer.SetPosition(1, info.edge.Item2);
            }*/

            // update startPoint
            startPoint = info.nextPoint;

            if (info.hasIntersection)
            {
                // debugging
                Debug.Log("in if statement: has intersection");

                // get corners of neighbouring triangle
                corners = GetNeighbouringTriangle(info.edge, info.lastCorner);

                // calculate normal of new triangle
                normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);

                /*
                // Debugging
                GameObject lineObject = new GameObject("LineRendererObject");
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

                lineRenderer.startWidth = 0.005f;
                lineRenderer.endWidth = 0.005f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.positionCount = 2;

                Vector3 normalStart = (corners.Item1 + corners.Item2 + corners.Item3) / 3f;
                lineRenderer.SetPosition(0, normalStart);
                lineRenderer.SetPosition(1, normalStart + normal);
                */

                // calculate step
                step = GetStepVector(stepDirection, normal, info.newStepSize, startPoint);

                /*
                // Debugging
                Debug.Log("calculate new step with normal: " + normal + " and stepSize: " + stepSize + " = " + step);

                // Debugging
                Debug.Log("calculating next theoretical point with: " + startPoint + " + " + step);
                */

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                /*
                // Debugging
                if (nextTheoreticalPoint == startPoint)
                {
                    Debug.Log("start and nextTheoretical point identical. calculation: " + startPoint + " + " + step);
                }
                */

                // update previousEdge
                previousEdge = info.edge;

                // Debugging
                Debug.Log("newStepSize: " + info.newStepSize);
                Debug.Log("stepSize: " + stepSize);

                // check if we've done a full step, if not, don't add point to path
                if(info.newStepSize == stepSize)
                {
                    // Debugging
                    Debug.Log("point is path point, intersection with edge: " + info.edge);

                    // add point to path
                    path[i] = startPoint;
                }
                else
                {
                    // Debugging
                    Debug.Log("point is debug point, intersection with edge: " + info.edge);

                    // add point to debug path
                    debugPath.Add(startPoint);

                    i--;
                }

            }
            else
            {
                // debugging
                Debug.Log("in if statement: has no intersection");

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // add new point to path
                path[i] = info.nextPoint;

            }
        }
    }

    // used to get a random start triangle
    public (Vector3, Vector3, Vector3) GetRandomTriangleOnMesh()
    {
        int rand = UnityEngine.Random.Range(0, triangleDict.Keys.Count);

        /*
        // Debugging
        int rand = 750;
        */

        var key = triangleDict.Keys.ElementAt(rand);
        Vector3 value = triangleDict[key][0];
        return (key.Item1, key.Item2, value);
    }


    // get start point on start triangle based on the three corners
    public Vector3 getStartPoint(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;

        // just using arbitrary values for mulitplication so point is inside the triangle
        Vector3 pointOnTriangle = a + 0.3f * ab + 0.3f * ac;

        return pointOnTriangle;
    }

    // calculate normal vector of triangle
    public Vector3 CalculateNormal (Vector3 a, Vector3 b, Vector3 c)
    {
        // sort the corners to find key in dictionary
        var sortedCorners = meshManager.GetVertexOrder(a, b, c);

        // look up the order that the corners should have for consistent normals across the mesh
        (Vector3, Vector3, Vector3) orderedCorners = sortedTrianglesDict[sortedCorners];

        Vector3 edge1 = orderedCorners.Item1 - orderedCorners.Item2;
        Vector3 edge2 = orderedCorners.Item1 - orderedCorners.Item3;

        Vector3 normal = Vector3.Cross(edge1, edge2);

        return normal;
    }

    // returns step vector parallel to triangle and with stepsize as length
    public Vector3 GetStepVector(Vector3 direction, Vector3 normal, float stepSize, Vector3 startPoint)
    {
        normal = normal.normalized;
        Vector3 projected = direction - (Vector3.Dot(direction, normal) * normal);

        // default up direction if normal and stepDirection are almost parallel
        if (projected.sqrMagnitude < 1e-6f) 
        {
            // Debugging
            Debug.Log("step direction replaced with up because otherwise it would be zero");

            projected = Vector3.up;  
        }

        Vector3 step = projected.normalized * stepSize;

        // Debugging
        Debug.Log("step with previous normal: " + step);

        return step;
    }

    // get all information needed to make the next step
    public IntersectionInfo NextStepInfo(Vector3 corner1, Vector3 corner2, Vector3 corner3, Vector3 stepStart, Vector3 stepEnd, (Vector3, Vector3)? previousEdge = null) 
    {
        // set up list of all corner combinations
        List<(Vector3, Vector3)> cornerCombinations = new List<(Vector3, Vector3)> { (corner1, corner2), (corner1, corner3), (corner2, corner3) };

        
        // remove previousEdge if it exists
        if (previousEdge.HasValue)
        {
            cornerCombinations.Remove(previousEdge.Value);

            //Debugging 
            foreach (var corner in cornerCombinations)
            {
                Debug.Log("number of edges after removing previous: " + cornerCombinations.Count);
                Debug.Log("edges after removign previous: " + corner);
            }
        }

        // Debugging
        Debug.Log("checking " + cornerCombinations.Count + " edges: " + cornerCombinations[0] + ", " + cornerCombinations[1]);

        // set up intersectionInfo
        IntersectionInfo info = new IntersectionInfo();

        // for each edge
        foreach ((Vector3, Vector3) edge in cornerCombinations)
        {
            // calculate the intersection 
            info = CalculateIntersectionInfo(edge.Item1, edge.Item2, stepStart, stepEnd); 

            // find last corner
            List<Vector3> corners = new List<Vector3> { corner1, corner2, corner3 };
            corners.Remove(edge.Item1);
            corners.Remove(edge.Item2);
            Vector3 lastCorner = corners[0];

            // if there is an intersection with an edge
            if (info.hasIntersection)
            {
                // Debugging
                Debug.Log("Has intersection with edge: (" + info.edge.Item1 + ", " + info.edge.Item2 + ")");

                // add lastCorner to info
                info.lastCorner = lastCorner;

                return info;
            }
        }

        // if there is no intersection with an edge

        // Debugging
        Debug.Log("no intersection with any edge");

        return info;
    }

    // returns information about the intersection of an edge and a step vector

    // sv1 = start of step vector
    // sv2 = end of step vector
    // ev1 = start of edge vector
    // ev2 = end of edge vector
    public IntersectionInfo CalculateIntersectionInfo(Vector3 ev1, Vector3 ev2, Vector3 sv1, Vector3 sv2) 
    {
        // set up direction vectors
        Vector3 ev = ev2 - ev1;
        Vector3 sv = sv2 - sv1;

        // Debugging
        Debug.Log("edge vector = " + ev1 + ", " + ev2 + ", = " + ev);
        Debug.Log("step vector = " + sv1 + ", " + sv2 + ", = " + sv);

        // calculate sv x ev
        Vector3 svXev = math.cross(sv, ev);

        // set up intersection info for when we don't intersect
        IntersectionInfo info = new IntersectionInfo()
        {
            startPoint = sv1,
            nextTheoreticalPoint = sv2,
            hasIntersection = false,
            nextPoint = sv1 + sv,
        };

        // if the crossproduct is zero, the vector are parallel or identical, both cases in which we want to count it as no intersection
        if (svXev == Vector3.zero)
        {
            // Debugging
            Debug.Log("no intersection because of cross product being zero");

            return info;
        }

        // calcualte (sv x ev) * (sv x ev)
        float svXevDOTsvXev = math.dot(svXev, svXev);

        // calculate ev1 - sv1
        Vector3 sv1ev1 = ev1 - sv1;

        // calcualte (ev1 - sv1) x ev
        Vector3 sv1ev1Xev = math.cross(sv1ev1, ev);

        // set up step intersection variable
        float s;

        // calculate s = ((ev1 - sv1) x ev * (sv x ev)) / ((sv x ev) * (sv x ev))
        s = math.dot(sv1ev1Xev, svXev) / svXevDOTsvXev;


        // calculate sv1 - ev1
        Vector3 ev1sv2 = sv1 - ev1;

        // calculate ev * ev
        float evDOTev = math.dot(ev, ev);

        // calculate t = (((sv1 - ev1) + sv * s) * ev) / (ev * ev)
        float t = math.dot(((sv1 - ev1) + (sv * s)), ev) / evDOTev;

        // if intersection is within bounds of line segment
        if (s >= -1e-6 && s <= 1 + 1e-6 && t >= -1e-6 && t <= 1 + 1e-6)
        {
            // calcualte nextPoint
            Vector3 nextPoint = sv1 + s * sv;

            // calculate new sv
            Vector3 newsv = nextPoint - sv1;

            // calculate |newsv|
            float newsvMAG = newsv.magnitude;

            // assign stepSize for return
            float remainingStep = stepSize;

            // Debugging 
            Debug.Log("s: " + s);

            // if we don't happen to do a full step to get to the intersection (account for fpp)
            if (Mathf.Abs(s - 1) > 1e-6f)
            {
                // reduce the stepSize (used for the next step)
                remainingStep = stepSize - newsvMAG;

                // Debugging
                Debug.Log("calculating remaning step size: " + stepSize + " - " + newsvMAG);
            }

            // get intersected edge in correct order
            var intersectedEdge = meshManager.GetEdgeOrder(ev1, ev2);

            // return intersection info
            IntersectionInfo intersectionInfo = new IntersectionInfo()
            {
                startPoint = sv1,
                nextTheoreticalPoint = sv2,
                hasIntersection = true,
                newStepSize = remainingStep,
                nextPoint = nextPoint,
                edge = intersectedEdge,
            };


            // Debugging
            Debug.Log("intersection is within bounds, therefor valid");
            
            return intersectionInfo;

        }

        // Debugging
        Debug.Log("intersection is not within bounds, therefore not valid");

        return info;

    }

    // returns neighbouring triangle
    public (Vector3, Vector3, Vector3) GetNeighbouringTriangle((Vector3, Vector3) edge, Vector3 corner)
    {
        List<Vector3> corners = triangleDict[edge];

        corners.Remove(corner);

        Vector3 lastCorner = corners[0];

        return (edge.Item1, edge.Item2, lastCorner);
        
    }

}
