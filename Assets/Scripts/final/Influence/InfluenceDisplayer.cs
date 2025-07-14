using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InfluenceDisplayer : MonoBehaviour
{
    private Dictionary<Influence, GameObject> spawnedInfluences = new();

    private void OnEnable()
    {
        InfluenceManager.OnInfluenceCreated += SpawnInfluence;
        InfluenceManager.OnInfluenceDeleted += DespawnInfluence;
    }

    private void OnDisable()
    {
        InfluenceManager.OnInfluenceCreated -= SpawnInfluence;
        InfluenceManager.OnInfluenceDeleted -= DespawnInfluence;
    }

    public void SpawnInfluence(Influence influence)
    {
        // debugging
        Debug.Log("influence spawned");

        GameObject instance = Instantiate(influence.Prefab, influence.Position, Quaternion.identity);
        instance.name = influence.Name;

        spawnedInfluences.Add(influence, instance);
    }

    public void DespawnInfluence(Influence influence)
    {
        // debugging
        Debug.Log("influence despawned");

        GameObject instance = spawnedInfluences[influence];
        Destroy(instance);
        spawnedInfluences.Remove(influence);
    }
}
