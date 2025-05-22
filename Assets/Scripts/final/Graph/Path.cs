using NUnit.Framework;
using UnityEngine;

public class Path
{
    public Node StartNode { get; }
    public Node EndNode { get; }
    public Vector3[] pathPoints { get; }
    public Sentence Sentence { get; set; }
    public Path(Node startNode, Node endNode, int numPathPoints)
    {
        StartNode = startNode;
        EndNode = endNode;
        pathPoints = new Vector3[numPathPoints];
    }
}
