using UnityEngine;

public class NodeComponent : MonoBehaviour
{
    public Node Node { get; private set; }
    public Graph Graph { get; private set; }

    public void Initialize(Node node, Graph graph)
    {
        Node = node;
        Graph = graph;
    }
}
