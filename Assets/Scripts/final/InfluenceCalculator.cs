using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class InfluenceCalculator
{
    public static List<float> CalculateInfluenceStrength(Vector3[] letterPositions, List<Influence> influences)
    {
        List<float> influenceStrengths = new();
        int numPointsInSphere = 0;
        int numPoints = letterPositions.Length;

        foreach(Influence influence in influences)
        {
            foreach(Vector3 point in letterPositions)
            {
                if(IsPointInSphere(point, influence.Position, influence.Radius))
                {
                    numPointsInSphere++;
                }

                float influenceStrength = numPointsInSphere / numPoints;

                influenceStrengths.Add(influenceStrength);
            }
        }

        return influenceStrengths;
    }

    public static string CalculateInfluencePrompt(List<float> InfluenceStrengths, List<Influence> influences)
    {
        if(influences.Count == 0)
        {
            return "";
        }

        string fullPrompt = "The sentence should be influenced ";

        for (int i = 0; i < influences.Count - 1; i++)
        {
            fullPrompt += "to " + InfluenceStrengths[i]*100 + " percent by '" + influences[i].PromptModifier + "', ";
        }
        fullPrompt += "and to " + InfluenceStrengths[influences.Count] * 100 + " percent by '" + influences[influences.Count].PromptModifier + "'.";

        return fullPrompt;
    }

    private static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
    {
        return Mathf.Pow(point.x - center.x, 2) + Mathf.Pow(point.y - center.y, 2)+ Mathf.Pow(point.z - center.z, 2) < Mathf.Pow(radius, 2);
    }
}
