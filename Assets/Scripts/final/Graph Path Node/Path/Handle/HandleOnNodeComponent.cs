using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HandleOnNodeComponent : MonoBehaviour
{
    public Node Node { get; private set; }
    public Handle Handle { get; private set; }

    private Vector3 lastPosition;

    public event Action<Node> OnHandleMoved;

    public void Initialize(Node node, Handle handle)
    {
        Node = node;
        Handle = handle;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if(lastPosition != transform.position)
        {
            lastPosition = transform.position;
            Node.Position = transform.position;
            Handle.Position = transform.position;

            List<Path> connectedPaths = new();
            connectedPaths.AddRange(Node.Incoming);
            connectedPaths.AddRange(Node.Outgoing);

            // debugging
            Debug.Log("connected paths: " + connectedPaths.Count);  

            foreach(Path path in connectedPaths)
            {
                HandleManager.Instance.UpdateSpline(path);
            }

            OnHandleMoved?.Invoke(Node);
        }
    }
}
