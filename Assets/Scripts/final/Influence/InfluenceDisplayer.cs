using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class InfluenceDisplayer : MonoBehaviour
{
    public static InfluenceDisplayer Instance { get; private set; }

    private Dictionary<Influence, GameObject> spawnedInfluences = new();

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

        // display radius
        GameObject radiusGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        radiusGO.transform.SetParent(instance.transform);
        radiusGO.transform.localPosition = Vector3.zero;

        float diameter = influence.Radius * 2;
        radiusGO.transform.localScale = new Vector3(diameter, diameter, diameter);

        Material radiusMaterial = Resources.Load<Material>("Materials/influenceRadiusM");
        radiusGO.GetComponent<Renderer>().material = radiusMaterial;

        SceneVisibilityManager.instance.DisablePicking(radiusGO, false); // makes it so this object can't be selected caues it gets in the way

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

    public void ToggleInfluenceVisibility(bool visible)
    {
        foreach(GameObject influenceGO in spawnedInfluences.Values)
        {
            influenceGO.SetActive(visible);
        }
    }

    public void ToggleInfluenceRadiusVisibility(bool visible)
    {
        foreach (GameObject influenceGO in spawnedInfluences.Values)
        {
            influenceGO.transform.GetChild(0).gameObject.SetActive(visible);
        }
    }
}
