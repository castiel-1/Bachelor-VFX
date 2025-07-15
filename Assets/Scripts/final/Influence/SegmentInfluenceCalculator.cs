using System.Collections.Generic;
using UnityEngine;

public static class SegmentInfluenceCalculator
{
    public static Dictionary<string, List<Influence>> CalculateInfluenceStrengths(Path path, List<Influence> influences)
    {
        Vector3 p0 = path.StartNode.Position;
        Vector3 p3 = path.EndNode.Position;

        //debugging
        Debug.Log("start of path for influence calc: " + p0);
        Debug.Log("end of path for influence calc: " + p3);

        Vector3 dir = p3 - p0;

        Vector3 p1 = p0 + (1f/3f) * dir;
        Vector3 p2 = p0 + (2f/3f) * dir;

        // path segments
        Dictionary<string, (Vector3 start, Vector3 end)> segments = new()
        {
            { "beginning", (p0, p1) },
            { "middle",    (p1, p2) },
            { "end",       (p2, p3) }
        };

        // output dictionary
        Dictionary<string, List<Influence>> result = new()
        {
            { "beginning", new List<Influence>() },
            { "middle",    new List<Influence>() },
            { "end",       new List<Influence>() }
        };

        foreach(var segment in segments)
        {
            // debugging
            Debug.Log("calcualting influence for segment: " +  segment);    

            Vector3 segStart = segment.Value.start;
            Vector3 segEnd = segment.Value.end;

            foreach(Influence influence in influences)
            {
                // debugging
                Debug.Log("influence with sphere interaction calculated");

                Vector3 closestPoint = CalculateClosestPointToLineSegment(segStart, segEnd, influence.Position);
                Vector3 closestPointToInfluenceCenter = closestPoint - influence.Position;
                float distance = closestPointToInfluenceCenter.magnitude;

                // debugging
                Debug.Log("closest point to influence center: " + closestPointToInfluenceCenter);
                Debug.Log("distance to center of sphere: " + distance);
                Debug.Log("influence radius: " + influence.Radius);

                if (distance <= influence.Radius)
                {
                    result[segment.Key].Add(influence);
                }
            }
        }

        return result;
        
    }

    private static Vector3 CalculateClosestPointToLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDir = lineEnd - lineStart;
        Vector3 startToPoint = point - lineStart;

        float lineLength = lineDir.magnitude;

        // t = how far along the line in percent the closest point is -> between 0 and 1 means it's on the line
        float t = Vector3.Dot(lineDir, startToPoint) / (lineLength * lineLength);

        Vector3 closestPoint = lineStart + Mathf.Clamp01(t) * lineDir; // we clamp t to 0 or 1 because we want the point to stay on the line segment (and this works for the intersection calculation as well)

        return closestPoint;
    }
}
