using Esper.ESave.SavableObjects;
using Esper.ESave;
using System.Collections.Generic;
using UnityEngine;

public static class GraphSaveAndLoad 
{
    public static void SaveGraph(Graph graph, string graphSaveKey, SaveFile saveFile)
    {
        // debugging
        Debug.Log("graph save key: " + graphSaveKey);

        GraphSaveData graphSaveData = ConvertToGraphSaveData(graph);

        Debug.Log("graph save data: " + graphSaveData);

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
        List<NodeSaveData> nodes = new();

        foreach (Node node in graph.Nodes)
        {
            NodeSaveData nodeSaveData = new NodeSaveData();
            nodeSaveData.ID = node.ID;
            nodeSaveData.position = node.Position;

            nodes.Add(nodeSaveData);
        }

        graphSaveData.Nodes = nodes;

        // convert list of paths
        List<PathSaveData> paths = new();

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

            paths.Add(pathSaveData);
        }

        graphSaveData.Paths = paths;

        return graphSaveData;
    }

    public static void LoadGraph(string graphSaveKey, SaveFile saveFile)
    {
        GraphSaveData graphSaveData = saveFile.GetData<GraphSaveData>(graphSaveKey);

        // create graph
        Graph graph = GraphManager.Instance.RecreateGraph();

        // create paths
        Dictionary<int, Node> nodesByID = new();
        foreach(NodeSaveData nodeSaveData in graphSaveData.Nodes)
        {
            Node node = GraphOperations.CreateNode(graph, nodeSaveData.position);
            node.ID = nodeSaveData.ID;
            nodesByID.Add(nodeSaveData.ID, node);
        }

        foreach(PathSaveData path in graphSaveData.Paths)
        {
            // start and end node
            Node startNode = nodesByID[path.startNodeID];
            Node endNode = nodesByID[path.endNodeID];

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
}
