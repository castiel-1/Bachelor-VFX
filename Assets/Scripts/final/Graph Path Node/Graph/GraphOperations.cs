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

    public static Path CreatePath(Graph graph, Node startNode, Node endNode)
    {
        // debugging
        Debug.Log("path created");

        Path nextPath = new Path(startNode, endNode);
        graph.Paths.Add(nextPath);

        startNode.Outgoing.Add(nextPath);
        endNode.Incoming.Add(nextPath);

        // add start and end as handles
        Handle startHandle = HandleOperations.CreateHandleOnNode(startNode, nextPath, true);
        Handle endHandle = HandleOperations.CreateHandleOnNode(endNode, nextPath, false);

        graph.RaisePathCreated(nextPath, graph);

        return nextPath;
    }

    public static void AddPathPoints(Graph graph, Path path, List<Vector3> pathPointPositions)
    {
        path.pathPoints.AddRange(pathPointPositions);

        graph.RaisePathPointsAdded(path, graph);
    }


    public static void DeletePath(Graph graph, Path path)
    {
        // debugging
        Debug.Log("path deleted");

        Node startNode = path.StartNode;
        Node endNode = path.EndNode;

        graph.Paths.Remove(path);
        startNode.Outgoing.Remove(path);
        endNode.Incoming.Remove(path);

        if (startNode.Outgoing.Count == 0 && startNode.Incoming.Count == 0)
        {
            DeleteNode(graph, startNode, path);
        }

        if (endNode.Outgoing.Count == 0 && endNode.Incoming.Count == 0)
        {
            DeleteNode(graph,endNode, path);
        }

        graph.RaisePathDeleted(path);
    }

    // only gets called when the node is owned by one path which is getting deleted
    private static void DeleteNode(Graph graph, Node node, Path path)
    {
        // debugging
        Debug.Log("node deleted");

        graph.Nodes.Remove(node);

        graph.RaiseNodeDeleted(node, path);
    }

    public static void TraverseBackwards(Graph graph, Node currentNode, int depth, int maxDepth, List<Path> currentBranch, List<List<Path>> allBranches)
    {
        if (depth >= maxDepth || currentNode.Incoming.Count == 0)
        {
            if(currentBranch.Count > 0)
            {
                List<Path> completeBranch = new List<Path>(currentBranch);
                completeBranch.Reverse();
                allBranches.Add(completeBranch);
            }
            return;
        }

        foreach (Path incomingPath in currentNode.Incoming)
        {
            Node incomingNode = incomingPath.StartNode;

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

