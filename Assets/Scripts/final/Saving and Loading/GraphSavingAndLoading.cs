using Esper.ESave.SavableObjects;
using Esper.ESave;
using System.Collections.Generic;
using UnityEngine;

public static class GraphSaveAndLoad 
{
    public static void SaveGraph(Graph graph, string graphSaveKey, SaveFile saveFile)
    {
        GraphSaveData graphSaveData = ConvertToGraphSaveData(graph);
        saveFile.AddOrUpdateData(graphSaveKey, graphSaveData);
        saveFile.Save();

        // debugging
        Debug.Log("graph saved");
    }

    private static GraphSaveData ConvertToGraphSaveData(Graph graph)
    {
        GraphSaveData graphSaveData = new GraphSaveData();

        // nodeID
        graphSaveData.NodeID = graph.NodeID;

        // convert list of nodes
        foreach (Node node in graph.Nodes)
        {
            NodeSaveData nodeSaveData = new NodeSaveData();
            nodeSaveData.ID = node.ID;
            nodeSaveData.position = node.Position;

            graphSaveData.Nodes.Add(nodeSaveData);
        }

        // convert list of paths
        foreach (Path path in graph.Paths)
        {
            PathSaveData pathSaveData = new PathSaveData();
            pathSaveData.startNodeID = path.StartNode.ID;
            pathSaveData.endNodeID = path.EndNode.ID;

            List<SavableVector> savablePathPoints = new();
            foreach (Vector3 pathPoint in path.pathPoints)
            {
                SavableVector savableVector = pathPoint;
                savablePathPoints.Add(savableVector);
            }
            pathSaveData.pathPoints = savablePathPoints;

            pathSaveData.sentenceText = path.Sentence.Text;

            graphSaveData.Paths.Add(pathSaveData);
        }
        return graphSaveData;
    }

    public static void LoadGraph(string graphSaveKey, SaveFile saveFile)
    {
        GraphSaveData graphSaveData = saveFile.GetData<GraphSaveData>(graphSaveKey);

        // create graph
        List<NodeSaveData> nodes = graphSaveData.Nodes;

        Vector3 startPosition = FindNodeByID(graphSaveData, 0).position;
        Vector3 endPosition = FindNodeByID(graphSaveData, 1).position;

        Graph graph = GraphManager.Instance.CreateGraph(startPosition, endPosition);

        // create paths
        foreach(PathSaveData path in graphSaveData.Paths)
        {
            // start and end node
            Vector3 startNodePosition = FindNodeByID(graphSaveData, path.startNodeID).position;
            Vector3 endNodePosition = FindNodeByID(graphSaveData, path.endNodeID).position;

            Node startNode = GraphOperations.CreateNode(graph, startNodePosition);
            Node endNode = GraphOperations.CreateNode(graph, endNodePosition);

            // pathPoints
            List<Vector3> pathPoints = new();

            foreach(SavableVector pathPoint in path.pathPoints)
            {
                Vector3 convertedPathPoint = pathPoint;
                pathPoints.Add(convertedPathPoint);
            }

            GraphOperations.RecreatePath(graph, startNode, endNode, path.sentenceText, pathPoints);  
        }
    }

    private static NodeSaveData FindNodeByID(GraphSaveData graph, int ID)
    {
        foreach (NodeSaveData node in graph.Nodes)
        {
            if (node.ID == ID)
            {
                return node;
            }
        }

        // debugging
        Debug.Log("node not found by ID");
        return null;
    }
}
