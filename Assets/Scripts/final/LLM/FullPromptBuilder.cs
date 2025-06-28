using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class FullPromptBuilder
{
    // depth = how far back we are considering for our history 
    public static string BuildPrompt(Graph graph, Path path, int numWords, int depth, InfluenceManager influenceManager)
    {
        string start = "Generate one sentence that is " + numWords + " words long. Your output should only be that sentence.";

        // build history prompt
        List<List<Path>> allBranches = GraphOperations.GetAllPreviousPaths(graph, path.StartNode, depth);
        string historyPrompt = HistoryPromptGenerator.GenerateHistoryPrompt(allBranches);

        // build influence prompt
        List<Influence> influences = (List<Influence>)influenceManager.Influences;
        Dictionary<string, List<Influence>> influenceStrengths = SegmentInfluenceCalculator.CalculateInfluenceStrengths(path, influences);
        string influencePrompt = SegmentInfluencePromptGenerator.CalculateInfluencePrompt(influenceStrengths);

        string fullPrompt = start + " " + historyPrompt + " " + influencePrompt;

        // debugging
        Debug.Log("full prompt: " + fullPrompt);

        return fullPrompt;
    }
}
