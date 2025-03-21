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

    public Vector3[] path;

   // private bool HadIntersection = false;
   // private (Vector3, Vector3) intersectionEdge = (Vector3.zero, Vector3.zero);

    public MeshManager meshManager;

    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();

    // Debugging
    Vector3 A;
    Vector3 B;
    Vector3 C;
    Vector3 D;

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
        /*
        // Debugging
        A = new Vector3(0.5f, 0.5f, 0.5f);
        B = new Vector3(-0.5f, 0.5f, 0.5f);
        C = new Vector3(0, 0.5f, 0.3f);
        D = new Vector3(0, 0.5f, 0.7f);

        bool intersection = Testing(A, B, C, D);

        Debug.Log("has intersection: " + intersection);
        */
        
        /*
        // set up path array
        int numLetters = buffer.text.Length;
        path = new Vector3[numLetters];

        // get triangleDictionary
        triangleDict = meshManager.GetNeighbouringTrianglesDict();


        // get all the variables we need to make a step
        var corners = GetRandomTriangleOnMesh();
        Vector3 startPoint = getStartPoint(corners.Item1, corners.Item2, corners.Item3);
        Vector3 normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);
        Vector3 step = GetStepVector(stepDirection, normal, stepSize);
        Vector3 nextTheoreticalPoint = startPoint + step;

        // add startPoint to path
        path[0] = startPoint;

        // for as many letters as we want to display (starting at 2 because of startPoint)
        for (int i = 1; i < numLetters; i++)
        {

            // calculate next point (either on the triangle or on an edge)
            IntersectionInfo info = CalculateIntersection(corners.Item1, corners.Item2, corners.Item3, startPoint, nextTheoreticalPoint);

            // update startPoint
            startPoint = info.nextPoint;
            
            // Debugging


            if (info.hasIntersection)
            {
                // debugging
                Debug.Log("has intersection");

                // get corners of neighbouring triangle
                corners = GetNeighbouringTriangle(info.edge, info.lastCorner);

                // calculate normal of new triangle
                normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);

                // calculate step
                step = GetStepVector(stepDirection, normal, info.newStepSize);

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // debugging
                if(nextTheoreticalPoint == startPoint)
                {
                    Debug.Log("start and nextTheoretical point identical. calculation: " + startPoint + " + " + step);
                }

                // add point to path
                path[i] = startPoint;

            }
            else
            {
                // debugging
                Debug.Log("has no intersection");

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // add new point to path
                path[i] = info.nextPoint;

            }
        }*/


        
    }

    /*
    public bool Testing(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        // set up variables for matrices
        Vector3 edgeStart = A;
        Vector3 edgeEnd = B;

        Vector3 edgeV = edgeEnd - edgeStart;

        Vector3 stepV = D - C;
        Vector3 startPoint = C;

        // set up matrices
        float2x2 matrixA = new float2x2(new float2(edgeV.x, edgeV.y), new float2(-stepV.x, -stepV.y));
        float2 matrixB = new float2(startPoint.x - edgeStart.x, startPoint.y - edgeStart.y);

        // check if there is an inverse
        float determinantA = matrixA.c0.x * matrixA.c1.y - matrixA.c1.x * matrixA.c0.y;

        // Debugging
        Debug.Log("determinant: " + determinantA);

        if(!(math.abs(determinantA) > 1e-6))
            {
            // Debugging
            Debug.Log("matrix inverse not found, no intersection with edge: " + edgeStart + ", " + edgeEnd);

            // go next edge if there is no inverse
            return false;
        }

        // calculate inverse
        float2x2 inverseA = math.inverse(matrixA);

        // calculate intersection
        float2 result = math.mul(inverseA, matrixB);
        float triangleInters = result.x;
        float stepInters = result.y;

        // if intersection is within bounds of line segments
        if (triangleInters >= 0 && triangleInters <= 1 && stepInters >= 0 && stepInters <= 1)
        {
            // Debugging
            Debug.Log("intersection within bounds");

            // calculate intersection position
            Vector3 intersection = startPoint + stepInters * stepV;

            // setup remaining stepSize
            float newStepSize = stepSize;

            // account for edge case where we land directly on the edge and don't want to reduce step size
            if (!(stepInters == 1))
            {
                // reduce remaining stepSize
                Vector3 newStep = intersection - startPoint;
                newStepSize = stepSize - newStep.magnitude;

            }

            // get edge in correct order
            var intersectedEdge = meshManager.GetEdgeOrder(edgeStart, edgeEnd);

            // set up struct for return
            IntersectionInfo intersectionInfo = new IntersectionInfo
            {
                nextPoint = intersection,
                edge = intersectedEdge,
                hasIntersection = true,
                newStepSize = newStepSize,
            };

            //Debugging
            Debug.Log("from: " + startPoint);
            Debug.Log("to: " + intersectionInfo.nextPoint);
            Debug.Log("with remaining step size: " + newStepSize);

            return intersectionInfo.hasIntersection;
        }

        // set up struct for return
        IntersectionInfo info = new IntersectionInfo
        {
            nextPoint = D,
            hasIntersection = false,
            newStepSize = stepSize,
        };

        // Debugging
        Debug.Log("from: " + startPoint);
        Debug.Log("to: " + info.nextPoint);

        return info.hasIntersection;

    }
    */


    // Debugging display of path
    private void OnDrawGizmos()
    {
        float r = 0.01f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A, r);
        Gizmos.DrawSphere(B, r);
        Gizmos.DrawSphere(C, r);
        Gizmos.DrawSphere(D, r);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(C, D);


        /*
        // display points
        Gizmos.color = Color.red;
        foreach (Vector3 point in path)
        {
            Gizmos.DrawSphere(point, 0.03f);
        }

        // draw lines between points
        Gizmos.color = Color.green;
        for (int i = 0; i < path.Length - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }*/

        
        /*
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPoint, 0.02f);
        */
    }
    // TODO

    // calculate one path
    public Vector3[] CreatePath()
    {
        return path;
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
    public Vector3 GetStepVector(Vector3 direction, Vector3 normal, float stepSize)
    {
        Vector3 projected = direction - (Vector3.Dot(direction, normal) * normal);
        Vector3 step = projected.normalized * stepSize;
        return step;
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

        // calculate sv x ev
        Vector3 svXev = math.cross(sv, ev);

        // set up intersection info for when we don't intersect
        IntersectionInfo info = new IntersectionInfo()
        {
            hasIntersection = false,
        };

        // if the crossproduct is zero, the vector are parallel or identical, both cases in which we want to count it as no intersection
        if (svXev == Vector3.zero)
        {
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

            // calculate |sv|
            float svMAG = sv.magnitude;

            // assign stepSize for return
            float remainingStep = stepSize;

            // if we don't happen to do a full step to get to the intersection
            if (!(s == 1))
            {
                // reduce the stepSize (used for the next step)
                remainingStep = stepSize - svMAG;
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

            return intersectionInfo;

        }

        return info;

    }


    /*
    // finds intersection information about the triangle and step
    public IntersectionInfo CalculateIntersection(Vector3 a, Vector3 b, Vector3 c, Vector3 startPoint, Vector3 nextPoint)
    {

        // list of all edges
        List<(Vector3, Vector3)> edges = new List<(Vector3, Vector3)> { (a,b) , (b,c), (c,a) };

        // step vector
        Vector3 stepV = nextPoint - startPoint;

        // for each edge
        foreach ((Vector3, Vector3) edge in edges)
        {
            // set up variables for matrices
            Vector3 edgeStart = edge.Item1;
            Vector3 edgeEnd = edge.Item2;

            Vector3 edgeV = edgeEnd - edgeStart;

            // set up matrices
            float2x2 matrixA = new float2x2(new float2(edgeV.x, edgeV.y), new float2(-stepV.x, -stepV.y));
            float2 matrixB = new float2(startPoint.x - edgeStart.x, startPoint.y - edgeStart.y);

            // check if there is an inverse
            float determinantA = matrixA.c0.x * matrixA.c1.y - matrixA.c1.x * matrixA.c0.y;

            if(!(math.abs(determinantA) > 1e-6))
            {
                // Debugging
                Debug.Log("matrix inverse not found, no intersection with edge: " + edgeStart + ", " + edgeEnd);

                // go next edge if there is no inverse
                continue;
            }

            // calculate inverse
            float2x2 inverseA = math.inverse(matrixA);

            // calculate intersection
            float2 result = math.mul(inverseA, matrixB);
            float triangleInters = result.x;
            float stepInters = result.y;

            // if intersection is within bounds of line segments
            if(triangleInters >= 0 && triangleInters <= 1 && stepInters >=0 && stepInters <= 1)
            {
                // Debugging
                Debug.Log("intersection within bounds");

                // calculate intersection position
                Vector3 intersection = startPoint + stepInters * stepV;

                // setup remaining stepSize
                float newStepSize = stepSize;

                // account for edge case where we land directly on the edge and don't want to reduce step size
                if(!(stepInters == 1))
                {
                    // reduce remaining stepSize
                    Vector3 newStep = intersection - startPoint;
                    newStepSize = stepSize - newStep.magnitude;
                   
                }

                // get last corner
                List<Vector3> allCorners = new List<Vector3> { a, b, c };
                allCorners.Remove(edgeStart);
                allCorners.Remove(edgeEnd);
                Vector3 lastCorner = allCorners[0];

                // get edge in correct order
                var intersectedEdge = meshManager.GetEdgeOrder(edgeStart, edgeEnd);

                // set up struct for return
                IntersectionInfo intersectionInfo = new IntersectionInfo
                {
                    nextPoint = intersection,
                    edge = intersectedEdge,
                    hasIntersection = true,
                    lastCorner = lastCorner,
                    newStepSize = newStepSize,
                };

                //Debugging
                Debug.Log("from: " + startPoint);
                Debug.Log("to: " + intersectionInfo.nextPoint);
                Debug.Log("intersection? " + intersectionInfo.hasIntersection);
                Debug.Log("with remaining step size: " + newStepSize);

                return intersectionInfo;
            }
        }

        // set up struct for return
        IntersectionInfo info = new IntersectionInfo
        {
            nextPoint = nextPoint,
            hasIntersection = false,
            newStepSize = stepSize,
        };

        // Debugging
        Debug.Log("from: " + startPoint);
        Debug.Log("to: " + info.nextPoint);

        return info;
    }
    */


    // returns neighbouring triangle
    public (Vector3, Vector3, Vector3) GetNeighbouringTriangle((Vector3, Vector3) edge, Vector3 corner)
    {
        List<Vector3> corners = triangleDict[edge];

        corners.Remove(corner);

        Vector3 lastCorner = corners[0];

        return (edge.Item1, edge.Item2, lastCorner);
        
    }

}
