using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GraphOperations 
{
    public static Node CreateNode(Graph graph, Vector3 position)
    {
        // debugging
        Debug.Log("node created");

        Node nextNode = new Node(graph.NodeID, position);
        graph.NodeID++;
        graph.Nodes.Add(nextNode);

        graph.RaiseNodeCreated(nextNode, graph);

        return nextNode;
    }

    public static Path CreatePath(Graph graph, Node startNode, Node endNode, int numPathPoints)
    {
        // debugging
        Debug.Log("path created");

        Path nextPath = new Path(startNode, endNode, numPathPoints);
        nextPath.pathPoints = SplineCalculator.CalculateSplinePoints(startNode.Position, endNode.Position, numPathPoints);
        graph.Paths.Add(nextPath);

        startNode.Outgoing.Add(endNode.ID);
        endNode.Incoming.Add(startNode.ID);

        graph.RaisePathCreated(nextPath);

        return nextPath;
    }

    public static void DeletePath(Graph graph, Path path)
    {
        // debugging
        Debug.Log("path deleted");

        Node startNode = path.StartNode;
        Node endNode = path.EndNode;

        graph.Paths.Remove(path);
        startNode.Outgoing.Remove(endNode.ID);
        endNode.Incoming.Remove(startNode.ID);

        if (startNode.Outgoing.Count == 0 && startNode.Incoming.Count == 0)
        {
            DeleteNode(graph, startNode);
        }

        if (endNode.Outgoing.Count == 0 && endNode.Incoming.Count == 0)
        {
            DeleteNode(graph,endNode);
        }

        graph.RaisePathDeleted(path);
    }

    private static void DeleteNode(Graph graph, Node node)
    {
        // debugging
        Debug.Log("node deleted");

        graph.Nodes.Remove(node);

        graph.RaiseNodeDeleted(node);
    }

    public static void TraverseBackwards(Graph graph, Node currentNode, int depth, int maxDepth, List<Path> currentBranch, List<List<Path>> allBranches)
    {
        if (depth >= maxDepth || currentNode.Incoming.Count == 0)
        {
            List<Path> completeBranch = new List<Path>(currentBranch);
            completeBranch.Reverse();
            allBranches.Add(completeBranch);
            return;
        }

        foreach (int incomingNodeID in currentNode.Incoming)
        {
            Node incomingNode = graph.Nodes.First(n => n.ID == incomingNodeID);
            Path incomingPath = graph.Paths.First(p => p.StartNode == incomingNode && p.EndNode == currentNode);

            currentBranch.Add(incomingPath);

            TraverseBackwards(graph, incomingNode, depth + 1, maxDepth, currentBranch, allBranches);

            currentBranch.RemoveAt(currentBranch.Count - 1);
        }
    }

    public static List<List<Path>> GetAllPreviousPaths(Graph graph, Node currentNode, int depth)
    {
        List<List<Path>> allBranches = new();

        TraverseBackwards(graph, currentNode, 0, depth, new List<Path>(), allBranches);

        return allBranches;
    }
}

