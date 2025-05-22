using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public List<Node> Nodes { get; private set; }
    public List<List<int>> AdjacencyList { get; private set; }
    public List<Path> Paths { get; private set; }
    public int ID;

   
    private int nodeID = 0;

    void Graph(int id)
    {
        ID = id;
        Nodes = new List<Node>();
        AdjacencyList = new List<List<int>>();
        Paths = new List<Path>();
    }

    public Path CreatePath(Vector3 startPosition, Vector3 endPosition, int numPathPoints)
    {
        Node startNode = CreateNode(startPosition);
        Node endNode = CreateNode(endPosition);

        Path nextPath = new Path(startNode, endNode, numPathPoints);
        
        AddPathToPaths(nextPath);
        AddPathToNeighbours(nextPath.StartNode, nextPath.EndNode);

        return nextPath;
    }

    private Node CreateNode(Vector3 position)
    {
        Node nextNode = new Node(nodeID, position);
        Nodes.Add(nextNode);
        AdjacencyList.Add(nextNode.Neighbours);
        nodeID++;

        return nextNode;
    }
    private void AddPathToPaths(Path nextPath)
    {
        Paths.Add(nextPath);
    }

    private void AddPathToNeighbours(Node startNode, Node endNode)
    {

        startNode.Neighbours.Add(endNode.ID);
    }

}
