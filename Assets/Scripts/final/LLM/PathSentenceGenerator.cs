using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEditor.Experimental.GraphView;

public class PathSentenceGenerator : MonoBehaviour
{
    public GraphManager graphManager;
    public LLMManager llmManager;

    private void OnEnable()
    {
        GraphOperations.OnPathCreated += HandlePathCreated;
    }

    private void OnDisable()
    {
        GraphOperations.OnPathCreated -= HandlePathCreated;
    }

    public async void HandlePathCreated(Path path, Graph graph)
    {
        // debugging
        Debug.Log("Handle path created called");

        // get full prompt
        int numWords = RandomizeNumberOfWords(RuntimeSettingsData.numberOfWordsMin, RuntimeSettingsData.numberOfWordsMax);
        string prompt = FullPromptBuilder.BuildPrompt(graph, path, numWords, RuntimeSettingsData.historyDepth, InfluenceManager.Instance);

        // call llm
        string llmOutput = await llmManager.PromptLLM(prompt);
        int outputLength = llmOutput.Length;

        // debugging
        Debug.Log("llm output received in handle path created");
        Debug.Log(llmOutput);
       
        // caluclate pathPoints
        List<Vector3> pathPointPositions = SplineCalculator.CalculateSplinePoints(path.StartNode.Position, path.EndNode.Position, outputLength);

        // debugging
        Debug.Log("number of pathPoints at calculation: " + pathPointPositions.Count);

        // add path points (which raises event to spawn them as well)
        GraphOperations.AddPathPoints(graph, path, pathPointPositions);

        // debugging
        Debug.Log("creating handle on start node...");
        Handle startHandle = HandleOperations.CreateHandleOnNode(path.StartNode, path, true);

        // debugging
        Debug.Log("creating handle on end node...");
        Handle endHandle = HandleOperations.CreateHandleOnNode(path.EndNode, path, false);

        // calculate sizes
        ITextSizeStrategy textSizeStrategy = TextSizeStrategyFactory.CreateTextSizeStrategy();
        float[] sizes = textSizeStrategy.GetTextSizes(outputLength);

        // add colour
        Color[] colors = new Color[outputLength];
        for (int i = 0; i < outputLength; i++)
        {
            colors[i] = RuntimeSettingsData.textColor;
        }

        // create buffer
        Sentence sentence = SentenceBufferManager.instance.AddSentence(llmOutput, path.pathPoints, sizes, null, null, colors);
        path.Sentence = sentence;

    }

    private int RandomizeNumberOfWords(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}
