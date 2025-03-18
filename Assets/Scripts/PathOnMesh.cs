using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

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

    private MeshManager meshManager;
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();

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

            if (info.hasIntersection)
            {

                // get corners of neighbouring triangle
                corners = GetNeighbouringTriangle(info.edge, info.lastCorner);

                // calculate normal of new triangle
                normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);

                // calculate step
                step = GetStepVector(stepDirection, normal, info.newStepSize);

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // add point to path
                path[i] = startPoint;

            }
            else
            {
                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // add new point to path
                path[i] = info.nextPoint;

            }
        }
       
    }

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

                // calculate intersection position
                Vector3 intersection = startPoint + stepInters * stepV;

                // setup remaining stepSize
                float newStepSize = stepSize;

                // account for edge case where we land directly on the edge and don't want to reduce step size
                if(!(stepInters == 1))
                {
                    // reduce remaining stepSize
                    newStepSize = stepSize - intersection.magnitude;
                }

                // get last corner
                List<Vector3> allCorners = new List<Vector3> { a, b, c };
                allCorners.Remove(edgeStart);
                allCorners.Remove(edgeEnd);
                Vector3 lastCorner = allCorners[0];

                // set up struct for return
                IntersectionInfo intersectionInfo = new IntersectionInfo
                {
                    nextPoint = intersection,
                    edge = (edgeStart, edgeEnd),
                    hasIntersection = true,
                    lastCorner = lastCorner,
                    newStepSize = newStepSize,
                };

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
