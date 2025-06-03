using System.Collections.Generic;
using UnityEngine;

// creates prompt based on count based influence calculation
public class CountBasedInfluencePromptGenerator : IInfluencePromptGenerator
{
    public string GenerateInfluencePrompt(List<Influence> influences, List<float> influenceStrengths)
    {
        if (influences.Count == 0)
        {
            return "";
        }

        string fullPrompt = "The sentence should be influenced ";

        if (influences.Count == 1)
        {
            fullPrompt += "to " + Mathf.RoundToInt(influenceStrengths[0] * 100) + " percent by '" + influences[0].PromptModifier + "'. ";

            return fullPrompt;
        }

        for (int i = 0; i < influences.Count; i++)
        {
            string part = "to " + Mathf.RoundToInt(influenceStrengths[i] * 100) + " percent by '" + influences[i].PromptModifier;

            if(i == influences.Count - 1)
            {
                fullPrompt += "and " + part + ".";
            }
            else if(i == influences.Count - 2)
            {
                fullPrompt += part + " ";
            }
            else
            {
                fullPrompt += part + ", ";
            }
        }

        return fullPrompt;
    }
}
