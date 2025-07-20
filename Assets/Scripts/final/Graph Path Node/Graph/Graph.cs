using System.Collections.Generic;
using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public int ID { get; private set; }
    public int NodeID { get; set; } = 0;
    public List<Node> Nodes { get; } = new();
    public List<Path> Paths { get; } = new();


    public event Action<Node, Graph> OnNodeCreated;
    public event Action<Node, Path> OnNodeDeleted;

    public event Action<Path, Graph> OnPathPointsAdded;

    public event Action<Path> OnPathDeleted;


    public void Initialize(int id)
    {
        ID = id;    
    }

    public void RaiseNodeCreated(Node node, Graph graph) => OnNodeCreated?.Invoke(node, graph);
    public void RaiseNodeDeleted(Node node, Path path) => OnNodeDeleted?.Invoke(node, path);
    public void RaisePathPointsAdded(Path path, Graph graph) => OnPathPointsAdded?.Invoke(path, graph);
    public void RaisePathDeleted(Path path) => OnPathDeleted?.Invoke(path);

}
