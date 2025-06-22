using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class HandleOperations
{
    public static event Action<Handle, Path> onHandleCreated;
    public static event Action<Handle> onHandleDestroyed;
    public static event Action<Path> onSplineUpdated;

    public static Handle CreateHandle(Vector3 position, Path path, int pathPointIndex, bool updateSpline = true)
    {
        // debugging
        Debug.Log("create handle called");

        Handle nextHandle = new Handle(position);

        // when we have no handles or one we are adding the start and end point handle which can be simplified
        if(path.Handles.Count < 2)
        {
            path.Handles.Add(nextHandle);
        }
        else
        {
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

        }
   
        
        Finish:

        if (updateSpline) // we don't want to update when adding the start and end node
        {
            UpdateSpline(path);
        }

        path.Handles.Add(nextHandle);

        onHandleCreated?.Invoke(nextHandle, path);

        return nextHandle;
    }

    public static void DeleteHandle(Handle handle, Path path)
    {
        // debugging
        Debug.Log("delete handle called");

        path.Handles.Remove(handle);

        UpdateSpline(path);

        onHandleDestroyed?.Invoke(handle);
    }

    public static void UpdateSpline(Path path)
    {
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

        onSplineUpdated?.Invoke(path);
    }
}
