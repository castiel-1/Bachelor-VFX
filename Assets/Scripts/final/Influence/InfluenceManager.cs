using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    public static event Action<SemanticInfluence> OnSemanticInfluenceCreated;
    public static event Action<SemanticInfluence> OnSemanticInfluenceDeleted;

    public static event Action<VisualInfluence> OnVisualInfluenceCreated;
    public static event Action<VisualInfluence> OnVisualInfluenceDeleted;

    private List<SemanticInfluence> semanticInfluences = new List<SemanticInfluence>();
    public IReadOnlyList<SemanticInfluence> SemanticInfluences => semanticInfluences;


    private List<VisualInfluence> visualInfluences = new List<VisualInfluence>();
    public IReadOnlyList<VisualInfluence> VisualInfluences => visualInfluences;

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
    public void AddSemanticInfluence(Vector3 position, string name, string promptModifier, float radius, GameObject prefab)
    {
        // debugging
        Debug.Log("semantci influence created");

        SemanticInfluence nextInfluence = InfluenceFactory.CreateSemanticInfluence(position, radius, promptModifier, name, prefab);
        semanticInfluences.Add(nextInfluence);
        OnSemanticInfluenceCreated?.Invoke(nextInfluence);
    }

    public void AddVisualInfluence(Vector3 position, string name, Color color, float radius, GameObject prefab)
    {
        // debugging
        Debug.Log("visual influence created");

        VisualInfluence nextInfluence = InfluenceFactory.CreateVisualInfluence(position, name, radius, color, prefab);
        visualInfluences.Add(nextInfluence);
        OnVisualInfluenceCreated?.Invoke(nextInfluence);
    }

    public void DeleteSemanticInfluence(SemanticInfluence influence)
    {
        // debugging
        Debug.Log("semantic influence deleted");

        semanticInfluences.Remove(influence);
        OnSemanticInfluenceDeleted?.Invoke(influence);
    }

    public void DeleteVisualInfluence(VisualInfluence influence)
    {
        // debugging
        Debug.Log("visual influence deleted");

        visualInfluences.Remove(influence);
        OnVisualInfluenceDeleted?.Invoke(influence);
    }

}
