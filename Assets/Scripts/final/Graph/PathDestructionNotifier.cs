using UnityEngine;
using System;

public class PathDestructionNotifier : MonoBehaviour
{
    public Path LinkedPath { get; set; }

    public static event Action<Path> OnPathDestroyed;
    private void OnDestroy()
    {
        OnPathDestroyed?.Invoke(LinkedPath);
    }
}
