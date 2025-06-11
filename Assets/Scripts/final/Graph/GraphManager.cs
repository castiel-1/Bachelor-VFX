using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance { get; private set; }

    private int graphID = 0;
    private Dictionary<int, GraphComponent> graphs = new();

    public GameObject graphPrefab;

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

    public GraphComponent CreateGraph()
    {
        // debugging
        Debug.Log("creating graph");

        GameObject graphGO = Instantiate(graphPrefab);
        graphGO.name = "gaph_" + graphID;

        GraphComponent graph = graphGO.GetComponent<GraphComponent>();

        graph.Initialize(graphID);

        graphs.Add(graphID, graph);

        graphID++;

        return graph;
    }

    public GraphComponent GetGraph(int graphID)
    {
        return graphs[graphID];
    }

    public void DeleteGraph(int graphID)
    {
        GraphComponent graph = graphs[graphID];
        graphs.Remove(graphID);

        Destroy(graph.gameObject);
    }

}
