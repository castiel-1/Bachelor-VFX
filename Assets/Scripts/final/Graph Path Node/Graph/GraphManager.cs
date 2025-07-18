using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance { get; private set; }

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

    private void OnEnable()
    {
        GraphDestructionNotifier.OnGraphGODestroyed += DeleteGraph;
    }

    private void OnDisable()
    {
        GraphDestructionNotifier.OnGraphGODestroyed -= DeleteGraph;
    }

    public Graph CreateGraph(Vector3 startPosition, Vector3 endPosition) // the position of the start and end node of the first path
    {
        // debugging
        Debug.Log("creating graph");

        GameObject graphGO = Instantiate(graphPrefab);
        graphGO.name = "gaph_" + graphID;
       
        Graph graph = graphGO.GetComponent<Graph>();

        GraphDestructionNotifier notifier = graphGO.AddComponent<GraphDestructionNotifier>();
        notifier.Graph = graph;

        graph.Initialize(graphID);

        graphs.Add(graphID, graph);

        graphID++;

        Node startNode = GraphOperations.CreateNode(graph, startPosition);
        Node endNode = GraphOperations.CreateNode(graph, endPosition);
        GraphOperations.CreatePath(graph, startNode, endNode);

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
        // debugging
        Debug.Log("delete graph called");

        // delete all sentences from buffer
        foreach(Path path in graph.Paths)
        {
            SentenceBufferManager.instance.DeleteSentence(path.Sentence);
        }

        // delete graph
        foreach (var pair in graphs)
        {
            if (pair.Value == graph)
            {
                graphs.Remove(pair.Key);
                Destroy(graph.gameObject);
                return;
            }
        }

    }
}
