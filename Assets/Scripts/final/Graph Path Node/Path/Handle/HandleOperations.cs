using System;
using UnityEngine;
using System.Collections.Generic;

public static class HandleOperations
{
    public static event Action<Handle, Path> onHandleCreated;
    public static event Action<Handle> onHandleDestroyed;

    public static Handle CreateHandle(Vector3 position, Path path)
    {
        // debugging
        Debug.Log("create handle called");

        Handle nextHandle = new Handle(position);

        path.Handles.Add(nextHandle);

        UpdateSpline(path);

        onHandleCreated?.Invoke(nextHandle, path);

        return nextHandle;
    }

    public static void DeleteHandle(Handle handle, Path path)
    {
        path.Handles.Remove(handle);

        UpdateSpline(path);

        onHandleDestroyed?.Invoke(handle);
    }

    public static void UpdateSpline(Path path)
    {
        // get the path handle belongs to, get all handles, recalculate
        List<Handle> handles = path.Handles;
        int numHandles = handles.Count;
        int numPathPoints = path.pathPoints.Length;

        if (numHandles < 2)
        {
            return;
        }

        int numSegments = handles.Count - 1;
        int pointsPerSegment = numPathPoints / numSegments;
        int leftOverPoints = numPathPoints % numSegments;

        List<Vector3> updatedPathPoints = new ();

        for (int i = 0; i < numSegments; i++)
        {
            int currentPointsInSegment = pointsPerSegment;

            Vector3 p0;
            Vector3 p1 = handles[i].Position;
            Vector3 p2 = handles[i + 1].Position;
            Vector3 p3;

            // on the first segment we need to interpolate p0
            if (i == 0)
            {
                p0 = p1 + (p2 - p1);
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

            // distribute leftover points
            if(leftOverPoints != 0)
            {
                currentPointsInSegment += 1;
                leftOverPoints -= 1;
            }

            // calculate segment points
            List<Vector3> segmentPoints = SplineCalculator.CalculateSplinePoints(p0, p1, p2, p3, currentPointsInSegment);

            updatedPathPoints.AddRange(segmentPoints);

        }

        // update path points
        path.pathPoints = updatedPathPoints.ToArray();
    }
}
