using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class InfluenceManager : MonoBehaviour
{
    public static event Action<Influence> OnInfluenceCreated;
    public static event Action<Influence> OnInfluenceDeleted;

    private List<Influence> influences = new List<Influence>();

    private void OnEnable()
    {
        InfluenceDestructionNotifier.OnInfluenceDestroyed += DeleteInfluence;
    }
    private void OnDisable()
    {
        InfluenceDestructionNotifier.OnInfluenceDestroyed -= DeleteInfluence;
    }

    public void CreateInfluence(Vector3 position, float radius, string promptModifier, GameObject prefab)
    {
        // debugging
        Debug.Log("influence created");

        Influence nextInfluence = new Influence(position, radius, promptModifier, prefab);
        influences.Add(nextInfluence);
        OnInfluenceCreated?.Invoke(nextInfluence);

    }

    public void DeleteInfluence(Influence influence)
    {
        // debugging
        Debug.Log("influence deleted");

        influences.Remove(influence);
        OnInfluenceDeleted?.Invoke(influence);
    }

}
