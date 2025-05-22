using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InfluenceManager : MonoBehaviour
{
    private List<Influence> influences = new List<Influence>();

    public void CreateInfluence(Vector3 position, float radius, string promptModifier, GameObject prefab)
    {
        Influence nextInfluence = new Influence(position, radius, promptModifier, prefab);
        influences.Add(nextInfluence);
    }

    public void DeleteInfluence(Influence influence)
    {
        influences.Remove(influence);
    }
}
