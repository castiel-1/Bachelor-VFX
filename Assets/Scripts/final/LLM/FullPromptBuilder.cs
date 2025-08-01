using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class FullPromptBuilder
{
    // depth = how far back we are considering for our history 
    public static string BuildPrompt(Graph graph, Path path, int numWords, int depth)
    {
        string start = "Generate one sentence that is " + numWords + " words long. Your output should only be that sentence.";

        // build history prompt
        List<List<Path>> allBranches = GraphOperations.GetAllPreviousPaths(graph, path.StartNode, depth);
        string historyPrompt = HistoryPromptGenerator.GenerateHistoryPrompt(allBranches);

        // build influence prompt
        List<SemanticInfluence> influences = InfluenceManager.Instance.SemanticInfluences;
        Dictionary<string, List<SemanticInfluence>> influenceStrengths = SegmentSemanticInfluenceCalculator.CalculateSemanticInfluenceStrengths(path, influences);
        string influencePrompt = SegmentSemanticInfluencePromptGenerator.CalculateInfluencePrompt(influenceStrengths);

        string fullPrompt = (start + " " + historyPrompt).Trim() + " " + influencePrompt; // trimming so there are no double spaces when history prompt is empty.

        // debugging
        Debug.Log("full prompt: " + fullPrompt);

        return fullPrompt;
    }
}
