using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Node
{
    public int ID { get; set; }
    public Vector3 Position { get; set; }
    public List<Path> Incoming {  get; }
    public List<Path> Outgoing { get; }

    public Node(int id, Vector3 position)
    {
        ID = id;
        Position = position;
        Incoming = new();
        Outgoing = new();
    }
}
