using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class HandleManager : MonoBehaviour
{
    public static event Action<Handle, Path> OnHandleOnPathCreated;
    public static event Action<Handle, Node> OnHandleOnNodeCreated;
    public static event Action<Handle> OnHandleOnNodeDestroyed;
    public static event Action<Handle> OnHandleOnPathDestroyed;
    public static event Action<Handle, Vector3, Path> OnHandleOnPathRecreated;
    public static event Action<bool> OnToggleAllHandles;
    public static event Action<Path> OnSplineUpdated;

    public static HandleManager Instance { get; private set; }

    private Dictionary<Node, Handle> nodeHandleDict = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Handle CreateHandleOnPath(Vector3 position, Path path, int pathPointIndex)
    {
        Handle nextHandle = new Handle(position);

        // find where to insert the handle
        int numSegments = path.HandlesWithIndeces.Count - 1;
        int numPathPoints = path.pathPoints.Count;
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
                path.HandlesWithIndeces[nextHandle] = pathPointIndex;
                // debugging
                Debug.Log("handle index when creating it: " + pathPointIndex);

                goto Finish;
            }

            runningIndex = currentEndIndex;
        }
   
        
        Finish:

        UpdateSpline(path);

        OnHandleOnPathCreated?.Invoke(nextHandle, path);

        return nextHandle;
    }

    public void MoveRecreatedHandleOnPath(Handle handle, Vector3 position, Path path)
    {
        handle.Position = position;

        OnHandleOnPathRecreated?.Invoke(handle, position, path);
    }

    public Handle CreateHandleOnNode(Node node, Path path, bool isStartNode)
    {
        // debugging
        Debug.Log("number of path points in create handle: " + path.pathPoints.Count);

        // if the node already has a handle
        if(nodeHandleDict.TryGetValue(node, out Handle handleAtNode))
        {
            // debugging
            Debug.Log("node already has handle");

            if (isStartNode)
            {
                // debugging
                Debug.Log("the handle is inserted for a startNode");

                path.HandlesWithIndeces[handleAtNode] = 0;
            }
            else
            {
                path.HandlesWithIndeces[handleAtNode] = path.pathPoints.Count;
            }

            return handleAtNode;
        }

        // if the node doesn't have a handle yet

        // debugging
        Debug.Log("node doesn't have a handle yet");
        Handle nextHandle;
        if (isStartNode)
        {
            nextHandle = new Handle(node.Position);
            path.HandlesWithIndeces[nextHandle] = 0;
        }
        else
        {
            // debugging
            Debug.Log("new handle has as index: " + path.pathPoints.Count);

            nextHandle = new Handle(node.Position); // numpathpoint.length because that is after the last pathPoint which is where the end Node is (doesnt have a pathPoint)
            path.HandlesWithIndeces[nextHandle] = path.pathPoints.Count;
        }

        nodeHandleDict[node] = nextHandle;

        OnHandleOnNodeCreated?.Invoke(nextHandle, node);
        return nextHandle;
    }

    public void DeleteHandleOnPath(Handle handle, Path path)
    {
        path.HandlesWithIndeces.Remove(handle);

        UpdateSpline(path);

        OnHandleOnPathDestroyed?.Invoke(handle);
    }

    public void DeleteHandleOnNode(Node node, Path path)
    {
        Handle handle = nodeHandleDict[node];
        path.HandlesWithIndeces.Remove(handle);

        OnHandleOnNodeDestroyed?.Invoke(handle);
    }

    public void UpdateSpline(Path path)
    {
        // debugging
        Debug.Log("updating spline");

        List<(Handle, int)> handlesAndIndices = path.HandlesWithIndeces
            .OrderBy(kv => kv.Value)
            .Select(kv => (kv.Key, kv.Value))
            .ToList();


        int numHandles = handlesAndIndices.Count;

        if (numHandles < 2)
        {
            return;
        }

        int numSegments = handlesAndIndices.Count - 1;

        List<Vector3> updatedPathPoints = new ();

        for (int i = 0; i < numSegments; i++)
        {
            int pointsInSegment = handlesAndIndices[i+1].Item2 - handlesAndIndices[i].Item2;

            // debugging
            Debug.Log("number of pointsInSegment: " +  pointsInSegment);

            Vector3 p0;
            Vector3 p1 = handlesAndIndices[i].Item1.Position;
            Vector3 p2 = handlesAndIndices[i + 1].Item1.Position;
            Vector3 p3;

            // on the first segment we need to interpolate p0
            if (i == 0)
            {
                p0 = p1 + (p1 - p2);
            }
            else
            {
                p0 = handlesAndIndices[i - 1].Item1.Position;
            }

            // on the last segment we need to interpolate p3
            if(i == (numSegments - 1))
            {
                p3 = p2 + (p2 - p1);
            }
            else
            {
                p3 = handlesAndIndices[i + 2].Item1.Position;
            }

            // calculate segment points
            List<Vector3> segmentPoints = SplineCalculator.CalculateSplinePoints(p0, p1, p2, p3, pointsInSegment);

            updatedPathPoints.AddRange(segmentPoints);

        }

        // update path points
        path.pathPoints = updatedPathPoints;

        OnSplineUpdated?.Invoke(path);
    }

    public void ToggleAllHandles(bool active)
    {
        OnToggleAllHandles?.Invoke(active);
    }
}
