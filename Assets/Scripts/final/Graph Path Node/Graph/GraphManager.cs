using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance { get; private set; }
    public event Action<Graph> OnGraphCreated;
    public event Action<Graph> OnGraphDeleted;

    private int graphID = 0;
    private Dictionary<int, Graph> graphs = new();

    public GameObject graphPrefab; // this holds a graph script and a graphDisplayer script

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Graph CreateGraph()
    {
        // debugging
        Debug.Log("creating graph");

        GameObject graphGO = Instantiate(graphPrefab);
        graphGO.name = "gaph_" + graphID;

        Graph graph = graphGO.GetComponent<Graph>();

        graph.Initialize(graphID);

        graphs.Add(graphID, graph);

        graphID++;

        OnGraphCreated?.Invoke(graph);

        return graph;
    }

    // debugging - this has been more or less replaced with reference based lookup but can still be useful for debugging so it stays here
    public Graph GetGraph(int graphID)
    {
        return graphs[graphID];
    }

    // debugging - this has been more or less replaced with reference based deletion but can still be useful for debugging so it stays here
    public void DeleteGraph(int graphID)
    {
        Graph graph = graphs[graphID];
        graphs.Remove(graphID);

        Destroy(graph.gameObject);
    }

    public void DeleteGraph(Graph graph)
    {
        foreach (var pair in graphs)
        {
            if (pair.Value == graph)
            {
                graphs.Remove(pair.Key);
                OnGraphDeleted?.Invoke(graph);
                Destroy(graph.gameObject);
                return;
            }
        }

    }
}
