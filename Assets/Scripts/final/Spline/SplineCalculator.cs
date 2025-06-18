using System.Collections.Generic;
using UnityEngine;

public static class SplineCalculator
{
    // 4 control points known
    public static List<Vector3> CalculateSplinePoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numSamplePoints)
    {
        List<Vector3> splinePoints = new ();

        for (int i = 0; i < numSamplePoints; i++)
        {
            float t = i / (float)numSamplePoints; // not numSamplePoints-1 because we exclude p2, so we can put the next sentence's first letter there
            splinePoints.Add(GetCatmullRomPoint(p0, p1, p2, p3, t));
        }

        return splinePoints;
    }

    // 2 control points known
    public static List<Vector3> CalculateSplinePoints(Vector3 p1, Vector3 p2, int numSamplePoints)
    {
        Vector3 p0 = p1 + (p1 - p2);
        Vector3 p3 = p2 + (p2 - p1);

        return CalculateSplinePoints(p0, p1, p2, p3, numSamplePoints);
    }

    private static Vector3 GetCatmullRomPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {

        Vector3 calculatedPoint =
        0.5f *
        ((2 * p1) +
        (-p0 + p2) * t +
        (2 * p0 - 5 * p1 + 4 * p2 - p3) * (t * t) +
        (-p0 + 3 * p1 - 3 * p2 + p3) * (t * t * t)
        );

        return calculatedPoint;
    }
}