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

        for (int i = 0; i < influences.Count - 1; i++)
        {
            fullPrompt += "to " + Mathf.RoundToInt(influenceStrengths[i] * 100) + " percent by '" + influences[i].PromptModifier + "', ";
        }
        fullPrompt += "and to " + Mathf.RoundToInt(influenceStrengths[influences.Count - 1] * 100) + " percent by '" + influences[influences.Count - 1].PromptModifier + "'.";

        return fullPrompt;
    }
}
