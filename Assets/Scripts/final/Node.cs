using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Node
{
    public int Index { get; }
    public Vector3 Position { get; }
    public List<Node> Neighbours {  get; }

    public Node(int index, Vector3 position)
    {
        Index = index;
        Position = position;
        Neighbours = new List<Node>();
    }
}
