using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GraphOperations 
{
    public static event Action<bool> OnToggleNodes;
    public static event Action<bool> OnTogglePathPoints;
    public static event Action<Path, Graph> OnPathCreated;
    public static event Action<Sentence> OnPathDeleted; // used to delete sentence
    public static event Action<Path, Graph, string> OnPathRecreated;

    public static Node CreateNode(Graph graph, Vector3 position)
    {
        Node nextNode = new Node(graph.NodeID, position);
        graph.NodeID++;
        graph.Nodes.Add(nextNode);

        graph.RaiseNodeCreated(nextNode, graph);

        return nextNode;
    }

    public static Path CreatePath(Graph graph, Node startNode, Node endNode)
    {
        Path nextPath = new Path(startNode, endNode);
        graph.Paths.Add(nextPath);

        startNode.Outgoing.Add(nextPath);
        endNode.Incoming.Add(nextPath);

        OnPathCreated?.Invoke(nextPath, graph);

        return nextPath;
    }

    public static Path RecreatePath(Graph graph, Node startNode, Node endNode, string sentenceText)
    {
        Path nextPath = new Path(startNode, endNode);
        graph.Paths.Add(nextPath);

        startNode.Outgoing.Add(nextPath);
        endNode.Incoming.Add(nextPath);

        OnPathRecreated?.Invoke(nextPath, graph, sentenceText);

        return nextPath;
    }

    public static void AddPathPoints(Graph graph, Path path, List<Vector3> pathPointPositions)
    {
        // debugging
        Debug.Log("added " + pathPointPositions.Count + " path points");

        path.pathPoints = pathPointPositions;

        graph.RaisePathPointsAdded(path, graph);
    }


    public static void DeletePath(Graph graph, Path path)
    {
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
        OnPathDeleted?.Invoke(path.Sentence);
    }

    // only gets called when the node is owned by one path which is getting deleted
    private static void DeleteNode(Graph graph, Node node, Path path)
    {
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

    public static void ToggleNodes(bool active)
    {
        OnToggleNodes?.Invoke(active);
    }

    public static void TogglePathPoints(bool active)
    {
        OnTogglePathPoints?.Invoke(active);
    }
}

