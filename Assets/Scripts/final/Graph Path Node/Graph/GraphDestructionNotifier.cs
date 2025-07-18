using System;
using UnityEngine;

public class GraphDestructionNotifier : MonoBehaviour
{
    public static Action<Graph> OnGraphGODestroyed;

    public Graph Graph { get; set; }

    private void OnDestroy()
    {
       OnGraphGODestroyed?.Invoke(Graph);
    }
}
