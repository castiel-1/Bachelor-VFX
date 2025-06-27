using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PathSentenceGenerator : MonoBehaviour
{
    public GraphManager graphManager;
    public LLMManager llmManager;

    private void OnEnable()
    {
        graphManager.OnGraphCreated += HandleGraphCreated;
        graphManager.OnGraphDeleted += HandleGraphDeleted;
    }

    private void OnDisable()
    {
        graphManager.OnGraphCreated -= HandleGraphCreated;
        graphManager.OnGraphDeleted -= HandleGraphDeleted;
    }

    public void HandleGraphCreated(Graph graph)
    {
        graph.OnPathCreated += HandlePathCreated;
    }

    public void HandleGraphDeleted(Graph graph)
    {
        graph.OnPathCreated -= HandlePathCreated;
    }

    public async void HandlePathCreated(Path path, Graph graph)
    {
        // debugging
        Debug.Log("Handle path created called");

        // call llm, then count, then add that much to buffer and calculatePathPoints


        string prompt = FullPromptBuilder.BuildPrompt(
            path.pathPoints.Length, graph, path, RuntimeSettingsData.historyDepth, 
            InfluenceManager.Instance, new CountBasedInfluenceCalculator(), new CountBasedInfluencePromptGenerator());

        string llmOutput = await llmManager.PromptLLM(prompt);

        // debugging
        Debug.Log("llm output received in handle path created");
        Debug.Log(llmOutput);

        // TODO deal with size and the dynamic size settings

        if(llmOutput.Length > path.pathPoints.Length)
        {
            Debug.LogError("too many letters from llm");
        }

        SentenceBufferManager.instance.AddSentence(llmOutput, path.pathPoints, 0.3f);
    }

}
