using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    public static event Action<Influence> OnInfluenceCreated;
    public static event Action<Influence> OnInfluenceDeleted;

    private List<Influence> influences = new List<Influence>();
    public IReadOnlyList<Influence> Influences => influences;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
