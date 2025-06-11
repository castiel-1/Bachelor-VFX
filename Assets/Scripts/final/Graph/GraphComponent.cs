using System.Collections.Generic;
using System;
using UnityEngine;

public class GraphComponent : MonoBehaviour
{
    public int ID { get; private set; }
    public int NodeID { get; set; } = 0;
    public List<Node> Nodes { get; } = new();
    public List<Path> Paths { get; } = new();

    

    public event Action<Node> OnNodeCreated;
    public event Action<Node> OnNodeDeleted;

    public event Action<Path> OnPathCreated;
    public event Action<Path> OnPathDeleted;

    public void Initialize(int id)
    {
        ID = id;    
    }

    public void RaiseNodeCreated(Node node) => OnNodeCreated?.Invoke(node);
    public void RaiseNodeDeleted(Node node) => OnNodeDeleted?.Invoke(node);
    public void RaisePathCreated(Path path) => OnPathCreated?.Invoke(path);
    public void RaisePathDeleted(Path path) => OnPathDeleted?.Invoke(path);

}
