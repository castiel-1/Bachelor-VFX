using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using System;

public class Graph : MonoBehaviour
{
    public GameObject nodePrefab;
    public List<Node> Nodes { get; }
    public List<List<int>> IncomingAdjacency { get; }
    public List<List<int>> OutgoingAdjacency { get; }
    public List<Path> Paths { get; private set; }
    public int ID { get; }

    public static event Action<Node> OnNodeCreated;

    public static event Action<Path> OnPathCreated;
    public static event Action<Path> OnPathDestroyed;

    private int nodeID = 0;

    public Graph(int id)
    {
        ID = id;
        Nodes = new List<Node>();
        IncomingAdjacency = new List<List<int>>();
        OutgoingAdjacency = new List<List<int>>();
        Paths = new List<Path>();
    }

    public void CreatePath(Node startNode, Node endNode, int numPathPoints)
    {
        Path nextPath = new Path(startNode, endNode, numPathPoints);
        
        nextPath.pathPoints = SplineCalculator.CalculateSplinePoints(startNode.Position, endNode.Position, numPathPoints);

        AddPathToPaths(nextPath);
        AddPathToIncomingOutgoing(nextPath.StartNode, nextPath.EndNode);

        OnPathCreated(nextPath);
    }

    public void DestroyPath(Path path)
    {
        Node startNode = path.StartNode;
        Node endNode = path.EndNode;

        Paths.Remove(path);

        RemovePathFromIncomingOutgoing(startNode, endNode);

        if (startNode.Outgoing.Count == 0 && startNode.Incoming.Count == 0)
        {
            DestroyNode(startNode);
        }

        if (endNode.Outgoing.Count == 0 && endNode.Incoming.Count == 0)
        {
            DestroyNode(endNode);
        }
 
        OnPathDestroyed(path);
    }

    public void CreateNode(Vector3 position)
    {
        Node nextNode = new Node(nodeID, position);
        Nodes.Add(nextNode);
        IncomingAdjacency.Add(nextNode.Incoming);
        OutgoingAdjacency.Add(nextNode.Outgoing);
        nodeID++;

        OnNodeCreated(nextNode);
    }

    public void DestroyNode(Node node)
    {

    }

    private void AddPathToPaths(Path nextPath)
    {
        Paths.Add(nextPath);
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
