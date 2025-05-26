using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using System;

public class Graph : MonoBehaviour
{
    public List<Node> Nodes { get; private set; }
    public List<List<int>> IncomingAdjacency { get; private set; }
    public List<List<int>> OutgoingAdjacency { get; private set; }
    public List<Path> Paths { get; private set; }
    public int ID { get; private set; }

    public static event Action<Node> OnNodeCreated;
    public static event Action<Node> OnNodeDeleted;

    public static event Action<Path> OnPathCreated;
    public static event Action<Path> OnPathDeleted;

    private int nodeID = 0;

    public void Initialize(int id)
    {
        ID = id;
    }
    private void Awake()
    {
        Nodes = new List<Node>();
        IncomingAdjacency = new List<List<int>>();
        OutgoingAdjacency = new List<List<int>>();
        Paths = new List<Path>();
    }

    private void OnEnable()
    {
        PathDestructionNotifier.OnPathDestroyed += DeletePath;
    }

    private void OnDisable()
    {
        PathDestructionNotifier.OnPathDestroyed -= DeletePath;
    }

    public void CreatePath(Node startNode, Node endNode, int numPathPoints)
    {
        // debugging
        Debug.Log("path created");

        Path nextPath = new Path(startNode, endNode, numPathPoints);
        
        nextPath.pathPoints = SplineCalculator.CalculateSplinePoints(startNode.Position, endNode.Position, numPathPoints);

        Paths.Add(nextPath);

        AddPathToIncomingOutgoing(nextPath.StartNode, nextPath.EndNode);

        OnPathCreated?.Invoke(nextPath);
    }

    public void DeletePath(Path path)
    {
        // debugging
        Debug.Log("path deleted");

        Node startNode = path.StartNode;
        Node endNode = path.EndNode;
        
        if(Paths == null)
        {
            Debug.Log("Paths is null");
        }
        Paths.Remove(path);

        RemovePathFromIncomingOutgoing(startNode, endNode);

        if (startNode.Outgoing.Count == 0 && startNode.Incoming.Count == 0)
        {
            DeleteNode(startNode);
        }

        if (endNode.Outgoing.Count == 0 && endNode.Incoming.Count == 0)
        {
            DeleteNode(endNode);
        }
 
        OnPathDeleted?.Invoke(path);
    }

    public Node CreateNode(Vector3 position)
    {
        // debugging
        Debug.Log("node created");

        Node nextNode = new Node(nodeID, position);
        Nodes.Add(nextNode);
        IncomingAdjacency.Add(nextNode.Incoming);
        OutgoingAdjacency.Add(nextNode.Outgoing);
        nodeID++;

        OnNodeCreated?.Invoke(nextNode);

        return nextNode;
    }

    public List<List<Path>> GetAllPreviousPaths(Node currentNode, int depth)
    {
        List<List<Path>> allBranches = new();

        TraverseBackwards(currentNode, 0, depth, new List<Path>(), allBranches);

        return allBranches;
    }

    private void TraverseBackwards(Node currentNode, int depth, int maxDepth, List<Path> currentBranch, List<List<Path>> allBranches)
    {
        if(depth >= maxDepth || currentNode.Incoming.Count == 0)
        {
            List<Path> completeBranch = new List<Path>(currentBranch);
            completeBranch.Reverse();
            allBranches.Add(completeBranch);
            return;
        }

        foreach (int incomingNodeID in currentNode.Incoming)
        {
            Node incomingNode = Nodes.First(n => n.ID ==  incomingNodeID);
            Path incomingPath = Paths.First(p => p.StartNode == incomingNode && p.EndNode == currentNode);

            currentBranch.Add(incomingPath);

            TraverseBackwards(incomingNode, depth + 1, maxDepth, currentBranch, allBranches);

            currentBranch.RemoveAt(currentBranch.Count - 1);
        }
    }

    private void DeleteNode(Node node)
    {
        // debugging
        Debug.Log("node deleted");

        Nodes.Remove(node);
        IncomingAdjacency.Remove(node.Incoming);
        OutgoingAdjacency.Remove(node.Outgoing);

        OnNodeDeleted?.Invoke(node);
    }

    private void AddPathToIncomingOutgoing(Node startNode, Node endNode)
    {
        startNode.Outgoing.Add(endNode.ID);
        endNode.Incoming.Add(startNode.ID);   
    }

    private void RemovePathFromIncomingOutgoing(Node startNode, Node endNode)
    {
        startNode.Outgoing.Remove(endNode.ID);
        endNode.Incoming.Remove(startNode.ID);
    }

    private int RandomizeNumPathPoints(int numPathPointsMin, int numPathPointsMax)
    {
        // min <= result < max + 1 
        int numPathPoints = UnityEngine.Random.Range(numPathPointsMin, numPathPointsMax + 1);
        return numPathPoints;
    }
}
