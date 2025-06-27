using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class SegmentInfluencePromptGenerator 
{
    public static string CalculateInfluencePrompt(Dictionary<string, List<Influence>> influencesPerSegment)
    {
        string prompt = "";

        foreach(string segment in influencesPerSegment.Keys)
        {
            string segmentPrompt = "The " + segment + " of the sentence should be influenced by ";

            foreach(Influence influence in  influencesPerSegment[segment])
            {
                segmentPrompt += influence.PromptModifier;
            }

            prompt += segmentPrompt;
        }

        return prompt;
    }

}
