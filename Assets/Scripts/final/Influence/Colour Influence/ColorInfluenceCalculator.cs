using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ColorInfluenceCalculator
{
    public static Color[] CalculateColorInfluences(Vector3[] pathPoints)
    {
        Color uninfluencedTextColor = RuntimeSettingsData.uninfluencedTextColor;
        Color[] colors = new Color[pathPoints.Length];
        List<ColorInfluence> colorInfluences = InfluenceManager.Instance.ColorInfluences;

        // debugging
        Debug.Log("number of color influences: " +  colorInfluences.Count);

        // default colour according to settings if no colour influences are there
        if(colorInfluences.Count == 0)
        {
            // debugging
            Debug.Log("no color influences found");

            for (int i = 0; i < pathPoints.Length; i++)
            {
                colors[i] = uninfluencedTextColor;
            }

            return colors;
        }

        for(int i = 0; i < pathPoints.Length; i++)
        {
            Vector3 point = pathPoints[i];
            Color mixedColor = Color.black;
            float totalWeight = 0f;

            foreach (ColorInfluence influence in colorInfluences)
            {
                float distance = Vector3.Distance(point, influence.Position);

                // if point is in sphere
                if (distance <= influence.Radius)
                {
                    // Debugging
                    Debug.Log("point in sphere");

                    float weight = 1f - (distance / influence.Radius);
                    mixedColor += influence.Color * weight;
                    totalWeight += weight;
                }
            }

            if (totalWeight > 0f)
            {
                colors[i] = mixedColor / totalWeight;
            }
            else
            {
                colors[i] = uninfluencedTextColor;
            }
        }

        return colors;
    }
}
