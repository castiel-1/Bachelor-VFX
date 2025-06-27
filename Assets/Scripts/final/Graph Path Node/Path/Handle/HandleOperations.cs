using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class HandleOperations
{
    public static event Action<Handle, Path> OnHandleOnPathCreated;
    public static event Action<Handle, Node> OnHandleOnNodeCreated;
    public static event Action<Handle> OnHandleOnNodeDestroyed;
    public static event Action<Handle> OnHandleOnPathDestroyed;
    public static event Action<bool> OnToggleAllHandles;
    public static event Action<Path> OnSplineUpdated;

    private static Dictionary<Node, Handle> nodeHandleDict = new();

    public static Handle CreateHandleOnPath(Vector3 position, Path path, int pathPointIndex)
    {
        // debugging
        Debug.Log("create handle on path called");

        Handle nextHandle = new Handle(position, pathPointIndex);

        // find where to insert the handle
        int numSegments = path.Handles.Count - 1;
        int numPathPoints = path.pathPoints.Length;
        int numPointsPerSegment = numPathPoints / numSegments;
        int leftOverPoints = numPathPoints % numSegments;
        int runningIndex = 0;

        // for each segment
        for (int i = 0; i < numSegments; i++)
        {
            // figure out how many points we have in this segment
            int currentSegmentPoints = numPointsPerSegment;

            if(leftOverPoints > 0)
            {
                numPointsPerSegment++;
                leftOverPoints--;
            }

            // add the numberOfPoitns in this segment to the beginning of this segment (e.g. if we are in segment 2 and we've already had 5 points, it's 5 + ...)
            int currentStartIndex = runningIndex;
            int currentEndIndex = runningIndex + currentSegmentPoints;

            // if the index of the handle we are inserting is between the start and end of this segment...
            if(pathPointIndex >= currentStartIndex && pathPointIndex < currentEndIndex)
            {
                path.Handles.Insert(i + 1, nextHandle);
                goto Finish;
            }

            runningIndex = currentEndIndex;
        }
   
        
        Finish:

        UpdateSpline(path);

        OnHandleOnPathCreated?.Invoke(nextHandle, path);

        return nextHandle;
    }

    public static Handle CreateHandleOnNode(Node node, Path path, bool isStartNode)
    {
        // debugging
        Debug.Log("create handle on node called");

        if(nodeHandleDict.TryGetValue(node, out Handle handleAtNode))
        {
            if (isStartNode)
            {
                path.Handles.Insert(0, handleAtNode);
            }
            else
            {
                path.Handles.Add(handleAtNode);
            }

            return handleAtNode;
        }

        Handle nextHandle;
        if (isStartNode)
        {
            nextHandle = new Handle(node.Position, 0);
            path.Handles.Insert(0, nextHandle);
        }
        else
        {
            nextHandle = new Handle(node.Position, path.pathPoints.Length); // numpathpoint.length because that is after the last pathPoint which is where the end Node is (doesnt have a pathPoint)
            path.Handles.Add(nextHandle);
        }

        nodeHandleDict[node] = nextHandle;

        OnHandleOnNodeCreated?.Invoke(nextHandle, node);
        return nextHandle;
    }

    public static void DeleteHandleOnPath(Handle handle, Path path)
    {
        // debugging
        Debug.Log("delete handle called");

        path.Handles.Remove(handle);

        UpdateSpline(path);

        OnHandleOnPathDestroyed?.Invoke(handle);
    }

    public static void DeleteHandleOnNode(Node node, Path path)
    {
        Handle handle = nodeHandleDict[node];
        path.Handles.Remove(handle);

        OnHandleOnNodeDestroyed?.Invoke(handle);
    }

    public static void UpdateSpline(Path path)
    {
        List<Handle> handles = path.Handles;
        int numHandles = handles.Count;

        if (numHandles < 2)
        {
            return;
        }

        int numSegments = handles.Count - 1;


        List<Vector3> updatedPathPoints = new ();

        for (int i = 0; i < numSegments; i++)
        {
            int pointsInSegment = handles[i+1].Index - handles[i].Index;

            Vector3 p0;
            Vector3 p1 = handles[i].Position;
            Vector3 p2 = handles[i + 1].Position;
            Vector3 p3;

            // on the first segment we need to interpolate p0
            if (i == 0)
            {
                p0 = p1 + (p1 - p2);
            }
            else
            {
                p0 = handles[i - 1].Position;
            }

            // on the last segment we need to interpolate p3
            if(i == (numSegments - 1))
            {
                p3 = p2 + (p2 - p1);
            }
            else
            {
                p3 = handles[i + 2].Position;
            }

            // calculate segment points
            List<Vector3> segmentPoints = SplineCalculator.CalculateSplinePoints(p0, p1, p2, p3, pointsInSegment);

            updatedPathPoints.AddRange(segmentPoints);

        }

        // update path points
        path.pathPoints = updatedPathPoints.ToArray();

        OnSplineUpdated?.Invoke(path);
    }

    public static void ToggleAllHandles(bool active)
    {
        OnToggleAllHandles?.Invoke(active);
    }
}
