using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class InfluenceManager : MonoBehaviour
{
    public static InfluenceManager Instance { get; private set; }

    public static event Action<Influence> OnInfluenceAdded;
    public static event Action<Influence> OnInfluenceDeleted;

    private List<SemanticInfluence> semanticInfluences = new List<SemanticInfluence>();
    public IReadOnlyList<SemanticInfluence> SemanticInfluences => semanticInfluences;


    private List<ColorInfluence> colorInfluences = new List<ColorInfluence>();
    public IReadOnlyList<ColorInfluence> ColorInfluences => colorInfluences;

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

    private void OnEnable()
    {
        InfluenceDestructionNotifier.OnInfluenceGODestroyed += DeleteInfluence;
    }

    private void OnDisable()
    {
        InfluenceDestructionNotifier.OnInfluenceGODestroyed -= DeleteInfluence;
    }

    public void AddSemanticInfluence(string name, Vector3 position, float radius, GameObject prefab, string promptModifier)
    {
        // debugging
        Debug.Log("semantci influence created");

        SemanticInfluence nextInfluence = InfluenceFactory.CreateSemanticInfluence(name, position, radius, prefab, promptModifier);
        semanticInfluences.Add(nextInfluence);
        OnInfluenceAdded(nextInfluence);
    }

    public void AddColorInfluence(string name, Vector3 position, float radius, GameObject prefab, Color color)
    {
        // debugging
        Debug.Log("color influence created");

        ColorInfluence nextInfluence = InfluenceFactory.CreateColorInfluence(name, position, radius, prefab, color);
        colorInfluences.Add(nextInfluence);
        OnInfluenceAdded(nextInfluence);
    }

    public void DeleteInfluence(Influence influence)
    {
        // debugging
        Debug.Log("influence deleted in manager");

        switch (influence)
        {
            case SemanticInfluence semanticInfluence:
                semanticInfluences.Remove(semanticInfluence);
                break;
            case ColorInfluence colorInfluence:
                colorInfluences.Remove(colorInfluence);
                break;
        }

        OnInfluenceDeleted?.Invoke(influence);
    }

}
