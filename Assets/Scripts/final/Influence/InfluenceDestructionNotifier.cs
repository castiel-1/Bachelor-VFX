using System;
using UnityEngine;

public class InfluenceDestructionNotifier : MonoBehaviour
{
    public static event Action<Influence> OnInfluenceGODestroyed;
    public Influence Influence { get; set; }

    private void OnDestroy()
    {
        OnInfluenceGODestroyed?.Invoke(Influence);
    }
}
