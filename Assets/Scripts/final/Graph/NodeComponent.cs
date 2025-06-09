using UnityEngine;

public class NodeComponent : MonoBehaviour
{
    public Node Node { get; private set; }

    public void Initialize(Node node)
    {
        Node = node;
    }
}
