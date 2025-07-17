using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ColorInfluenceCalculator
{
    public static Color[] CalculateColorInfluences(Vector3[] pathPoints)
    {
        Color[] colors = new Color[pathPoints.Length];
        VisualInfluence[] colorInfluences = InfluenceManager.Instance.VisualInfluences.ToArray();

        // default colour is black if no colour influences are there
        if(colorInfluences.Length == 0)
        {
            // debugging
            Debug.Log("no color influences found");

            for (int i = 0; i < pathPoints.Length; i++)
            {
                colors[i] = Color.black;
            }
        }


        for(int i = 0; i < pathPoints.Length; i++)
        {
            Color? color = null; // null to begin with because if there is only one influence color we want that to show cleanly and not be muddy with black

            for (int j = 0; j < colorInfluences.Length; j++)
            {
                if (IsPointInSphere(pathPoints[i], colorInfluences[j].Position, colorInfluences[j].Radius))
                {
                    // if there is no color, first influence defines color
                    if(color == null)
                    {
                        color = colorInfluences[j].Color;
                    }
                    // otherwise mix colors
                    else
                    {
                        color = Color.Lerp(color.Value, colorInfluences[j].Color, 0.5f);
                    }
                }
                else
                {
                    color = Color.black;
                }
            }

            colors[i] = color.Value;
        }

        return colors;
    }

    private static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
    {
        float result = Mathf.Pow((point.x - center.x), 2) + Mathf.Pow((point.y - center.y), 2) + Mathf.Pow((point.z - center.z), 2);

        float radiusSquared = Mathf.Pow(radius, 2);

        if(result <= radiusSquared)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
