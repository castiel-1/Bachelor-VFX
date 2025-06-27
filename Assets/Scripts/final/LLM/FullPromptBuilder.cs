using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class FullPromptBuilder
{
    public static string BuildPrompt(
        int numPoints, 
        Graph graph, Path path, int depth, // how far back we are considering for our history
        InfluenceManager influenceManager, IInfluenceCalculator influenceCalculator, IInfluencePromptGenerator influencePromptGenerator)
    {
        string start = "Generate one sentence with exactly " + numPoints + " characters counting letters, spaces and punctuation marks. Your output should only be that sentence.";

        // build history prompt
        List<List<Path>> allBranches = GraphOperations.GetAllPreviousPaths(graph, path.StartNode, depth);
        string historyPrompt = HistoryPromptGenerator.GenerateHistoryPrompt(allBranches);

        // build influence prompt
        List<Influence> influences = (List<Influence>)influenceManager.Influences;
        List<float> influenceStrengths = influenceCalculator.CalculateInfluenceStrengths(path.pathPoints, influences);
        string influencePrompt = influencePromptGenerator.GenerateInfluencePrompt(influences, influenceStrengths);

        string fullPrompt = start + " " + historyPrompt + " " + influencePrompt;

        // debugging
        Debug.Log("full prompt: " + fullPrompt);

        return fullPrompt;
    }
}
