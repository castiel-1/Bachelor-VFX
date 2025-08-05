using System;
using System.Collections.Generic;
using Esper.ESave;
using Esper.ESave.SavableObjects;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance { get; private set; }
    private List<Graph> graphs = new();
    private int graphID = 0;

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

        GameObject parent = GameObject.Find("User Creation");
        graph.transform.SetParent(parent.transform);

        graphs.Add(graph);

        graphID++;

        Node startNode = GraphOperations.CreateNode(graph, startPosition);
        Node endNode = GraphOperations.CreateNode(graph, endPosition);

        GraphOperations.CreatePath(graph, startNode, endNode);

        return graph;
    }

    public Graph RecreateGraph()
    {
        // debugging
        Debug.Log("recreating graph");

        GameObject graphGO = Instantiate(graphPrefab);
        graphGO.name = "gaph_" + graphID;

        Graph graph = graphGO.GetComponent<Graph>();

        GraphDestructionNotifier notifier = graphGO.AddComponent<GraphDestructionNotifier>();
        notifier.Graph = graph;

        graph.Initialize(graphID);

        GameObject parent = GameObject.Find("User Creation");
        graph.transform.SetParent(parent.transform);

        graphs.Add(graph);

        graphID++;

        return graph;
    }

    public void DeleteGraph(Graph graph)
    {
        // debugging
        Debug.Log("delete graph called");

        if( graph!= null)
        {
            // delete all sentences from buffer
            foreach (Path path in graph.Paths)
            {
                SentenceBufferManager.Instance.DeleteSentence(path.Sentence);
            }

            // delete graph
            graphs.Remove(graph);
            Destroy(graph.gameObject);
            return;
        }

    }
    public List<Graph> GetGraphs()
    {
        return graphs;
    }

}
