using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    public Mesh mesh;
    public Vector3 stepDirection;
    public float stepSize;
    public GraphicsInfoBuffer buffer;

   // private bool HadIntersection = false;
   // private (Vector3, Vector3) intersectionEdge = (Vector3.zero, Vector3.zero);

    public MeshManager meshManager;

    private Vector3[] path;
    private List<Vector3> debugPath = new List<Vector3>();
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();

    // Debugging
    public GameObject vertex;

    // struct for intersection information
    public struct IntersectionInfo
    {
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
            Gizmos.DrawSphere(point, 0.03f);
        }

        Gizmos.color = Color.blue;
        // display all points
        foreach(Vector3 point in debugPath)
        {
            Gizmos.DrawSphere(point, 0.03f);
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

        // get all the variables we need to make the initial step
        var corners = GetRandomTriangleOnMesh();
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

                // get furthest corner from next trianlge (so we can orient our step in the right direction)
                List<Vector3> cornerList = new List<Vector3> { corners.Item1, corners.Item2, corners.Item3 };
                cornerList.Remove(info.edge.Item1);
                cornerList.Remove(info.edge.Item2);
                Vector3 furhtestCorner = cornerList[0];

                // calculate step
                step = GetStepVector(stepDirection, normal, info.newStepSize, startPoint, furhtestCorner);

                // Debugging
                Debug.Log("calculate new step with normal: " + normal + " and stepSize: " + stepSize + " = " + step);

                // Debugging
                Debug.Log("calculating next theoretical point with: " + startPoint + " + " + step);

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // debugging
                if (nextTheoreticalPoint == startPoint)
                {
                    Debug.Log("start and nextTheoretical point identical. calculation: " + startPoint + " + " + step);
                }

                // update previousEdge
                previousEdge = info.edge;

                // Debugging
                Debug.Log("newStepSize: " + info.newStepSize);
                Debug.Log("stepSize: " + stepSize);

                // check if we've done a full step, if not, don't add point to path
                if(info.newStepSize == stepSize)
                {
                    // add point to path
                    path[i] = startPoint;
                }
                else
                {
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
        Vector3 ab = b - a;
        Vector3 ac = c - a;

        Vector3 normal = Vector3.Cross(ab, ac);

        return normal;
    }

    // returns step vector parallel to triangle and with stepsize as length
    public Vector3 GetStepVector(Vector3 direction, Vector3 normal, float stepSize, Vector3 startPoint, Vector3? furhtestCorner = null)
    {
        Vector3 projected = direction - (Vector3.Dot(direction, normal) * normal);

        // default up direction if normal and stepDirection are almost parallel
        if (projected.sqrMagnitude < 1e-6f) 
        {
            projected = Vector3.up;  
        }

        Vector3 step = projected.normalized * stepSize;


        if (furhtestCorner.HasValue)
        {
            // check distance to opposite corner so we don't accidentally step away from our triangle
            float currentDistance = Vector3.Distance(startPoint, furhtestCorner.Value);
            float newDistance = Vector3.Distance(startPoint + step, furhtestCorner.Value);

            // if we step away, we reverse the step vector
            if (newDistance > currentDistance)
            {
                step = -step;
            }
        }
       
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
        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
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
