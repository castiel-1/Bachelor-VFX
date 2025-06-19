using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Path
{
    public Node StartNode { get; }
    public Node EndNode { get; }
    public Vector3[] pathPoints { get; set; }
    public Sentence Sentence { get; set; }
    public List<Handle> Handles { get; set; }

    public Path(Node startNode, Node endNode, int numPathPoints)
    {
        StartNode = startNode;
        EndNode = endNode;
        pathPoints = new Vector3[numPathPoints];
        Handles = new List<Handle>();
    }
}
