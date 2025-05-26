using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

// calculates influence based on how many points are in influence sphere
public class CountBasedInfluenceCalculator : IInfluenceCalculator
{
    public List<float> CalculateInfluenceStrengths(Vector3[] letterPositions, List<Influence> influences)
    {
        List<float> influenceStrengths = new();
        int numPoints = letterPositions.Length;

        foreach(Influence influence in influences)
        {
            int numPointsInSphere = 0;

            foreach (Vector3 point in letterPositions)
            {
                if(IsPointInSphere(point, influence.Position, influence.Radius))
                {
                    numPointsInSphere++;
                }
            }

            float influenceStrength = (float)numPointsInSphere / numPoints;

            // debugging
            Debug.Log("calculated Strength = " + numPointsInSphere + " / " + numPoints + " = " + influenceStrength);

            influenceStrengths.Add(influenceStrength);
        }

        return influenceStrengths;
    }

    private static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
    {
        return Mathf.Pow(point.x - center.x, 2) + Mathf.Pow(point.y - center.y, 2)+ Mathf.Pow(point.z - center.z, 2) < Mathf.Pow(radius, 2);
    }
}
