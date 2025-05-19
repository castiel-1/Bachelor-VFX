using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PathCalculator : MonoBehaviour
{
    public float originalStepSize;
    public GraphicsInfoBuffer buffer;
    public MeshHandler meshHandler;


    private Vector3[] pathPoints;
    private PathInfo[] pointInfos;
    private float currentStepSize;

    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();
    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();


    // struct for intersection information
    public struct IntersectionInfo
    {
        public Vector3 startPoint;
        public Vector3 nextPoint;
        public (Vector3, Vector3) edge;
        public Vector3 lastCorner;
        public bool hasIntersection;
        public float newStepSize;
    }

    // struct for path information
    public struct PathInfo
    {
        public Vector3 point;
        public Vector3 normal;
        public Vector3 lineDirection;
    }

    // testing
    public Node startNode;
    public Node endNode;
    public int numLetters;
    public int startIndex; // the index of the triangle where we start until we figure out how to exactly  use the start point for that

    void Awake()
    {
        // create path
        CalculateLetterPositions(startNode, endNode, numLetters);
    }

    Vector3[] CalculateLetterPositions(Node startNode, Node endNode, int numLetters)
    {
        // set up path array
        pathPoints = new Vector3[numLetters];

        // set up pathInfo array
        pointInfos = new PathInfo[numLetters];

        // set up currentStepSize
        currentStepSize = originalStepSize;

        // get triangleDictionary
        triangleDict = meshHandler.GetNeighbouringTrianglesDict();

        // get sortedTrianglesDcitionary
        sortedTrianglesDict = meshHandler.GetSortedTrianglesDict();

        // get all the variables we need to make the initial step
        var corners = sortedTrianglesDict.Keys.ElementAt(startIndex);

        Vector3 startPoint = GetStartPoint(corners.Item1, corners.Item2, corners.Item3); // TEMPORARY
        Vector3 normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);
        Vector3 stepDirection = startNode.Position - endNode.Position;
        if (AreOpposite(stepDirection, normal))
        {
            stepDirection = Vector3.up;
        }

        Vector3 step = GetStepVector(stepDirection, normal, originalStepSize, startPoint);
        Vector3 nextTheoreticalPoint = startPoint + step;

        // set up previousEdge
        (Vector3, Vector3)? previousEdge = null;

        // add startPoint to path
        pathPoints[0] = startPoint;

        // add startPoint to pathInfo
        PathInfo currentPathInfo = new PathInfo()
        {
            point = startPoint,
            normal = normal,
            lineDirection = nextTheoreticalPoint - startPoint,
        };

        pointInfos[0] = currentPathInfo;

        // set up edge for step calculation
        Vector3 edge = Vector3.zero;

        // for as many letters as we want to display (starting at 2 because of startPoint)
        for (int i = 1; i < numLetters; i++)
        {

            // calculate next point (either on the triangle or on an edge)
            IntersectionInfo info = NextStepInfo(corners.Item1, corners.Item2, corners.Item3, startPoint, nextTheoreticalPoint, previousEdge);

            // update startPoint
            startPoint = info.nextPoint;

            // update stepDirection
            stepDirection = info.nextPoint - info.startPoint;
            if (AreOpposite(stepDirection, normal))
            {
                stepDirection = Vector3.up;
            }

            if (info.hasIntersection)
            {
                // get corners of neighbouring triangle
                corners = GetNeighbouringTriangle(info.edge, info.lastCorner);

                // calculate normal of new triangle
                normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);

                // update lineDirection in currentPathInfo
                currentPathInfo.lineDirection = info.nextPoint - info.startPoint;

                // update normal in currentPathInfo
                currentPathInfo.normal = normal;

                // update stepSize
                currentStepSize = info.newStepSize;

                // if a full step has been taken
                if (Mathf.Abs(currentStepSize) < 1e-6)
                {
                    // reset currentStepSize
                    currentStepSize = originalStepSize;
                }

                // calculate step
                var cornerNextTriangle = meshHandler.GetNeighbouringTriangleLastCorner(info.edge, info.lastCorner);
                var orderedEdge = meshHandler.GetEdgeOrderOnTriangle(info.edge.Item1, info.edge.Item2, cornerNextTriangle);
                edge = orderedEdge.Item2 - orderedEdge.Item1;
                step = GetStepVector(stepDirection, normal, currentStepSize, startPoint, edge);

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // update previousEdge
                previousEdge = info.edge;

                if (currentStepSize == originalStepSize)
                {
                    // add point to path
                    pathPoints[i] = startPoint;

                    // add point to pathInfo
                    currentPathInfo.point = startPoint;
                    pointInfos[i] = currentPathInfo;
                }
                else
                {
                    i--;
                }

            }
            else
            {

                // update currentStepSize (always resets when we don't intersect cause then we do a full step or complete one)
                currentStepSize = originalStepSize;

                // if we have a previously intersected edge
                if (edge != Vector3.zero)
                {
                    // calculate step with edge
                    step = GetStepVector(stepDirection, normal, currentStepSize, startPoint, edge);
                }
                else
                {
                    // calculate step without edge
                    step = GetStepVector(stepDirection, normal, currentStepSize, startPoint);
                }

                // calculate next theoretical point
                nextTheoreticalPoint = startPoint + step;

                // add new point to path
                pathPoints[i] = startPoint;

                // add point to pathInfo
                currentPathInfo.point = startPoint;
                pointInfos[i] = currentPathInfo;

            }
        }

        return pathPoints;
    }

    bool AreOpposite(Vector3 a, Vector3 b)
    {
        a.Normalize();
        b.Normalize();
        return Mathf.Approximately(Vector3.Dot(a, b), -1f);
    }


    // TEMPORARY
    public Vector3 GetStartPoint(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;

        // just using arbitrary values for mulitplication so point is inside the triangle
        Vector3 pointOnTriangle = a + 0.3f * ab + 0.3f * ac;

        return pointOnTriangle;
    }

    // calculate normal vector of triangle
    public Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        // sort the corners to find key in dictionary
        var sortedCorners = meshHandler.GetCornerOrder(a, b, c);

        // look up the order that the corners should have for consistent normals across the mesh
        (Vector3, Vector3, Vector3) orderedCorners = sortedTrianglesDict[sortedCorners];

        Vector3 edge1 = orderedCorners.Item2 - orderedCorners.Item1;
        Vector3 edge2 = orderedCorners.Item3 - orderedCorners.Item2;

        Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

        return normal;
    }

    // returns step vector parallel to triangle and with stepsize as length 
    // IMPORTANT: EDGE NEEDS TO BE THE CORRECT ORIENTATION WHEN HANDED TO THIS FUNCTION
    public Vector3 GetStepVector(Vector3 direction, Vector3 normal, float stepSize, Vector3 startPoint, Vector3? edge = null)
    {
        Vector3 projected = direction - (Vector3.Dot(direction, normal) * normal);


        // TODO this should no longer be needed since we are now using the previous stepdireciton instead of a global one
        // default up direction if normal and stepDirection are almost parallel
        if (projected.sqrMagnitude < 1e-6f)
        {
            Vector3 a = new Vector3(0, 1, 0.2f);
            a = a.normalized;
            projected = a - (Vector3.Dot(a, normal) * a);
        }

        Vector3 step = projected.normalized * stepSize;

        // TODO this should no longer be necessary since we are now using the previous direction
        // see if we need to flip step direction when we start from the edge of a triangle
        if (edge.HasValue)
        {
            // get inward pointing normal of edge
            Vector3 inwardNormal = math.cross(edge.Value, normal);

            float dot = Vector3.Dot(step, inwardNormal);

            // if the dot product is greater than zero we need to flip the step vector
            if (dot > 0)
            {
                // we mirror vector along the edge we just came from so it still points in the correct direction while going into the triangle
                Vector3 projection = (math.dot(step, edge.Value) / math.dot(edge.Value, edge.Value)) * edge.Value;
                step = 2f * projection - step;

            }

        }

        return step;
    }

    // get all information needed to make the next step
    public IntersectionInfo NextStepInfo(Vector3 corner1, Vector3 corner2, Vector3 corner3, Vector3 stepStart, Vector3 stepEnd, (Vector3, Vector3)? previousEdge = null)
    {
        // get correct order of corners
        var cornersKey = meshHandler.GetCornerOrder(corner1, corner2, corner3);
        var orderedCorners = sortedTrianglesDict[cornersKey];

        // set up list of all corner combinations
        List<(Vector3, Vector3)> cornerCombinations = new List<(Vector3, Vector3)> {
            (orderedCorners.Item1, orderedCorners.Item2),
            (orderedCorners.Item2, orderedCorners.Item3),
            (orderedCorners.Item3, orderedCorners.Item1),
        };

        // remove previousEdge if it exists
        if (previousEdge.HasValue)
        {
            // get correct order of edge
            List<Vector3> corners = new List<Vector3> { corner1, corner2, corner3 };
            corners.Remove(previousEdge.Value.Item1);
            corners.Remove(previousEdge.Value.Item2);
            Vector3 lastCorner = corners[0];

            var orderedPreviousEdge = meshHandler.GetEdgeOrderOnTriangle(previousEdge.Value.Item1, previousEdge.Value.Item2, lastCorner);

            // remove edge from list we check for intersections
            cornerCombinations.Remove(orderedPreviousEdge);

        }

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
                // add lastCorner to info
                info.lastCorner = lastCorner;

                return info;
            }
        }

        // if there is no intersection with an edge
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

        // calculate sv x ev
        Vector3 svXev = math.cross(sv, ev);

        // set up intersection info for when we don't intersect
        IntersectionInfo info = new IntersectionInfo()
        {
            startPoint = sv1,
            hasIntersection = false,
            nextPoint = sv1 + sv,
        };

        // if the crossproduct is zero, the vector are parallel or identical, both cases in which we want to count it as no intersection
        if (svXev.magnitude < 1e-8f)
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
        if (s > 1e-6 && s <= 1 + 1e-6 && t > 1e-6 && t <= 1 + 1e-6)
        {
            // calcualte nextPoint
            Vector3 nextPoint = sv1 + s * sv;

            // calculate new sv
            Vector3 newsv = nextPoint - sv1;

            // calculate |newsv|
            float newsvMAG = newsv.magnitude;

            // reduce the step size used for the next step
            float remainingStep = currentStepSize - newsvMAG;

            // TODO THIS SHOULD NOT BE NECESSARY
            // get intersected edge in correct order
            var intersectedEdge = meshHandler.GetEdgeOrder(ev1, ev2);

            // return intersection info
            IntersectionInfo intersectionInfo = new IntersectionInfo()
            {
                startPoint = sv1,
                hasIntersection = true,
                newStepSize = remainingStep,
                nextPoint = nextPoint,
                edge = intersectedEdge,
            };

            return intersectionInfo;

        }

        return info;

    }

    // returns neighbouring triangle
    public (Vector3, Vector3, Vector3) GetNeighbouringTriangle((Vector3, Vector3) edge, Vector3 corner)
    {
        // order edge correctly
        var orderedEdge = meshHandler.GetEdgeOrder(edge.Item1, edge.Item2);

        List<Vector3> corners = new List<Vector3>(triangleDict[orderedEdge]);

        corners.Remove(corner);

        Vector3 lastCorner = corners[0];

        // Debugging
        // technically not needed but better to order it anyways - remove later
        var correctOrder = meshHandler.GetCornerOrder(edge.Item1, edge.Item2, lastCorner);

        return correctOrder;

    }


}


