using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using Unity.VisualScripting;

public class PathSentenceGenerator : MonoBehaviour
{
    public static PathSentenceGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void OnEnable()
    {
        GraphOperations.OnPathCreated += HandlePathCreated;
        GraphOperations.OnPathRecreated += HandlePathRecreated;
    }

    private void OnDisable()
    {
        GraphOperations.OnPathCreated -= HandlePathCreated;
        GraphOperations.OnPathRecreated -= HandlePathRecreated;
    }

    public async void HandlePathCreated(Path path, Graph graph)
    {
        // debugging
        Debug.Log("Handle path created called");

        // get full prompt
        int numWords = RandomizeNumberOfWords(RuntimeSettingsData.numberOfWordsMin, RuntimeSettingsData.numberOfWordsMax);
        string prompt = FullPromptBuilder.BuildPrompt(graph, path, numWords, RuntimeSettingsData.historyDepth);

        // call llm
        string llmOutput = await LLMManager.Instance.PromptLLM(prompt);
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
        Handle startHandle = HandleManager.Instance.CreateHandleOnNode(path.StartNode, path, true);

        // debugging
        Debug.Log("creating handle on end node...");
        Handle endHandle = HandleManager.Instance.CreateHandleOnNode(path.EndNode, path, false);

        // calculate sizes
        ITextSizeStrategy textSizeStrategy = TextSizeStrategyFactory.CreateTextSizeStrategy();
        float[] sizes = textSizeStrategy.GetTextSizes(outputLength);

        // add colour
        Color[] colors = ColorInfluenceCalculator.CalculateColorInfluences(pathPointPositions.ToArray());

        // caluclate line directions
        Vector3[] lineDirections = OrientationOperations.CalculateLineDirections(pathPointPositions.ToArray());

        // create buffer
        Sentence sentence = SentenceBufferManager.Instance.AddSentence(llmOutput, path.pathPoints, sizes, RuntimeSettingsData.rotation, lineDirections, colors);
        path.Sentence = sentence;

    }

    public void HandlePathRecreated(Path path, Graph graph, string sentenceText)
    {
        // debugging
        Debug.Log("handle path recreated called");

        // caluclate pathPoints
        List<Vector3> pathPointPositions = SplineCalculator.CalculateSplinePoints(path.StartNode.Position, path.EndNode.Position, sentenceText.Length);

        // debugging
        Debug.Log("number of pathPoints at calculation: " + pathPointPositions.Count);

        // add path points (which raises event to spawn them as well)
        GraphOperations.AddPathPoints(graph, path, pathPointPositions);

        // debugging
        Debug.Log("creating handle on start node...");
        Handle startHandle = HandleManager.Instance.CreateHandleOnNode(path.StartNode, path, true);

        // debugging
        Debug.Log("creating handle on end node...");
        Handle endHandle = HandleManager.Instance.CreateHandleOnNode(path.EndNode, path, false);

        // calculate sizes
        ITextSizeStrategy textSizeStrategy = TextSizeStrategyFactory.CreateTextSizeStrategy();
        float[] sizes = textSizeStrategy.GetTextSizes(path.pathPoints.Count);

        // add colour
        Color[] colors = ColorInfluenceCalculator.CalculateColorInfluences(path.pathPoints.ToArray());

        // caluclate line directions
        Vector3[] lineDirections = OrientationOperations.CalculateLineDirections(pathPointPositions.ToArray());

        // create buffer
        Sentence sentence = SentenceBufferManager.Instance.AddSentence(sentenceText, path.pathPoints, sizes, RuntimeSettingsData.rotation, lineDirections, colors);
        path.Sentence = sentence;

    }

    private int RandomizeNumberOfWords(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

}
