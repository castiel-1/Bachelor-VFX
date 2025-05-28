using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FullPromptBuilder : MonoBehaviour
{
    public string BuildPrompt(
        int numPoints, 
        Graph graph, Node currentNode, int depth,
        InfluenceManager influenceManager, IInfluenceCalculator influenceCalculator, Vector3[] letterPositions, IInfluencePromptGenerator influencePromptGenerator)
    {
        string start = "Generate one sentence with " + numPoints + " words. You output should only be that sentence.";

        // build history prompt
        List<List<Path>> allBranches = graph.GetAllPreviousPaths(currentNode, depth);
        string historyPrompt = HistoryPromptGenerator.GenerateHistoryPrompt(allBranches);

        // build influence prompt
        List<Influence> influences = (List<Influence>)influenceManager.Influences;
        List<float> influenceStrengths = influenceCalculator.CalculateInfluenceStrengths(letterPositions, influences);
        string influencePrompt = influencePromptGenerator.GenerateInfluencePrompt(influences, influenceStrengths);

        return start + " " + historyPrompt + " " + influencePrompt;
    }
}
