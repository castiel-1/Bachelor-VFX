using System.Collections.Generic;
using Esper.ESave;
using Unity.VisualScripting;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour 
{
    public static SaveAndLoadManager Instance { get; private set; }

    private SaveFileSetup saveFileSetup;
    private SaveFile saveFile;

    int numGraphs;
    private string graphSaveKeyStart = "graphSaveKey";
    private string semanticInfluencesSaveKey = "semanticInfluencesSaveKey";
    private string colorInfluencesSaveKey = "colorInfluencesSaveKey";

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        saveFileSetup = GetComponent<SaveFileSetup>();
        saveFile = saveFileSetup.GetSaveFile();
    }

    public void SaveScene()
    {
        // save graphs
        List<Graph> graphs = GraphManager.Instance.GetGraphs();
        numGraphs = graphs.Count;

        foreach(Graph graph in graphs)
        {
            string graphSaveKey = $"{graphSaveKeyStart}_{graph.ID}";
            GraphSaveAndLoad.SaveGraph(graph, graphSaveKey, saveFile);
        }

        // save influences
        List<SemanticInfluence> semanticInfluences = new List<SemanticInfluence>(InfluenceManager.Instance.SemanticInfluences);
        InfluenceSaveAndLoad.SaveSemanticInfluences(semanticInfluences, semanticInfluencesSaveKey, saveFile);   

        List<ColorInfluence> colorInfluences = new List<ColorInfluence>(InfluenceManager.Instance.ColorInfluences);
        InfluenceSaveAndLoad.SaveColorInfluences(colorInfluences, colorInfluencesSaveKey, saveFile);

        // debugging
        Debug.Log("saved everything");
    }

    public void LoadScene()
    {
        // load graphs
        for (int i = 0; i < numGraphs; i++)
        {
            GraphSaveAndLoad.LoadGraph($"{graphSaveKeyStart}_{i}", saveFile);
        }

        // load influences
        InfluenceSaveAndLoad.LoadSemanticInfluences(semanticInfluencesSaveKey, saveFile);
        InfluenceSaveAndLoad.LoadColorInfluences(colorInfluencesSaveKey, saveFile);
    }
}
