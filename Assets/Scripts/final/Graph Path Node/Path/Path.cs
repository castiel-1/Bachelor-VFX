using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Path
{
    public Node StartNode { get; }
    public Node EndNode { get; }
    public List<Vector3> pathPoints { get; set; }
    public Sentence Sentence { get; set; }
    public Dictionary<Handle, int> HandlesWithIndeces { get; set; }

    public Path(Node startNode, Node endNode)
    {
        StartNode = startNode;
        EndNode = endNode;
        pathPoints = new();
        HandlesWithIndeces = new();
    }
}
