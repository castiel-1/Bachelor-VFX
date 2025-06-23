using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HandleOnNodeComponent : MonoBehaviour
{
    public Node Node { get; private set; }
    public Vector3 lastPosition;

    public void Initialize(Node node)
    {
        Node = node;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if(lastPosition != transform.position)
        {
            lastPosition = transform.position;
            Node.Position = transform.position;

            List<Path> connectedPaths = new();
            connectedPaths.AddRange(Node.Incoming);
            connectedPaths.AddRange(Node.Outgoing);

            foreach(Path path in connectedPaths)
            {
                HandleOperations.UpdateSpline(path);
            }
        }
    }
}
