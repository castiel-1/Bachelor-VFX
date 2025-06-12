using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Node
{
    public int ID { get; }
    public Vector3 Position { get; }
    public List<int> Incoming {  get; }
    public List<int> Outgoing { get; }

    public Node(int id, Vector3 position)
    {
        ID = id;
        Position = position;
        Incoming = new List<int>();
        Outgoing = new List<int>();
    }
}
