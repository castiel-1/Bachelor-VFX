using UnityEngine;
using System;

public class InfluenceDestructionNotifier : MonoBehaviour
{
    public Influence LinkedInfluence { get; set; }

    public static event Action<Influence> OnInfluenceDestroyed;
    private void OnDestroy()
    {
        OnInfluenceDestroyed?.Invoke(LinkedInfluence);
    }
}
