using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class InfluenceCalculator
{
    public static List<float> CalculateInfluenceStrength(Vector3[] letterPositions, List<Influence> influences)
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

    public static string CalculateInfluencePrompt(List<float> InfluenceStrengths, List<Influence> influences)
    {
        if(influences.Count == 0)
        {
            return "";
        }

        string fullPrompt = "The sentence should be influenced ";

        if(influences.Count == 1)
        {
            fullPrompt += "to " + Mathf.RoundToInt(InfluenceStrengths[0] * 100) + " percent by '" + influences[0].PromptModifier + "'. ";

            return fullPrompt;
        }

        for (int i = 0; i < influences.Count - 1; i++)
        {
            fullPrompt += "to " + Mathf.RoundToInt(InfluenceStrengths[i] * 100) + " percent by '" + influences[i].PromptModifier + "', ";
        }
        fullPrompt += "and to " + Mathf.RoundToInt(InfluenceStrengths[influences.Count - 1] * 100) + " percent by '" + influences[influences.Count - 1].PromptModifier + "'.";

        return fullPrompt;
    }

    private static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
    {
        return Mathf.Pow(point.x - center.x, 2) + Mathf.Pow(point.y - center.y, 2)+ Mathf.Pow(point.z - center.z, 2) < Mathf.Pow(radius, 2);
    }
}
