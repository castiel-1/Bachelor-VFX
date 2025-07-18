using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class InfluenceDisplayer : MonoBehaviour
{
    public static InfluenceDisplayer Instance { get; private set; }

    private Dictionary<SemanticInfluence, GameObject> spawnedSemanticInfluences = new();
    private Dictionary<ColorInfluence, GameObject> spawnedColorInfluences = new();

    private Transform semanticParentTransform;
    private Transform colorParentTransform;

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

        GameObject rootGO = new GameObject("Influences");

        GameObject semanticGO = new GameObject("Semantic Influences");
        semanticGO.transform.SetParent(rootGO.transform, false);
        semanticParentTransform = semanticGO.transform;

        GameObject visualGO = new GameObject("Visual Influences");
        visualGO.transform.SetParent(rootGO.transform, false);
        colorParentTransform = visualGO.transform;
    }

    private void OnEnable()
    {
        InfluenceManager.OnInfluenceAdded += SpawnInfluence;
        InfluenceManager.OnInfluenceDeleted += DespawnInfluence;
    }

    private void OnDisable()
    {
        InfluenceManager.OnInfluenceAdded -= SpawnInfluence;
        InfluenceManager.OnInfluenceDeleted -= DespawnInfluence;
    }

    public void SpawnInfluence(Influence influence)
    {
        switch (influence)
        {
            case SemanticInfluence semanticInfluence:
                GameObject semanticGO = SpawnSemanticInfluence((SemanticInfluence) influence);
                InfluenceDestructionNotifier semanticNotifier = semanticGO.AddComponent<InfluenceDestructionNotifier>();
                semanticNotifier.Influence = semanticInfluence;
                InfluencePositionTracker semanticPositionTracker = semanticGO.AddComponent<InfluencePositionTracker>();
                semanticPositionTracker.Influence = semanticInfluence;
                spawnedSemanticInfluences.Add((SemanticInfluence) semanticInfluence, semanticGO);
                break;
            case ColorInfluence colorInfluence:
                GameObject colorGO = SpawnColorInfluence((ColorInfluence) colorInfluence);
                InfluenceDestructionNotifier colorNotifier = colorGO.AddComponent<InfluenceDestructionNotifier>();
                colorNotifier.Influence = colorInfluence;
                InfluencePositionTracker colorPositionTracker = colorGO.AddComponent<InfluencePositionTracker>();
                colorPositionTracker.Influence = colorInfluence;
                spawnedColorInfluences.Add(colorInfluence, colorGO);
                break;
        }
    }

    private GameObject SpawnSemanticInfluence(SemanticInfluence influence)
    {
        GameObject instance = Instantiate(influence.Prefab, influence.Position, Quaternion.identity);
        instance.name = influence.Name;
        instance.transform.SetParent(semanticParentTransform);

        // spawn radius
        Material radiusMaterial = Resources.Load<Material>("Materials/semanticInfluenceRadiusM");
        GameObject radiusGO = SpawnRadius(influence.Radius, radiusMaterial, instance.transform);

        return instance;
    }

    private GameObject SpawnColorInfluence(ColorInfluence influence)
    {
        GameObject instance = Instantiate(influence.Prefab, influence.Position, Quaternion.identity);
        instance.name = influence.Name;
        instance.transform.SetParent(colorParentTransform);

        // changing material colour 
        Material baseVisualMaterial = Resources.Load<Material>("Materials/visualInfluenceM");
        Material visualMaterial = new Material(baseVisualMaterial);
        visualMaterial.color = influence.Color;
        instance.GetComponent<Renderer>().material = visualMaterial;    

        // spawn radius
        Material baseMaterial = Resources.Load<Material>("Materials/visualInfluenceRadiusM");
        Material radiusMaterial = new Material(baseMaterial); // clone base material so colour changes can be made per visual influence
        radiusMaterial.color = new Color(influence.Color.r, influence.Color.g, influence.Color.b, 0.4f);
        GameObject radiusGO = SpawnRadius(influence.Radius, radiusMaterial, instance.transform);

        return instance;
    }

    private GameObject SpawnRadius(float radius, Material material, Transform parentInfluenceSphere)
    {
        // display radius
        GameObject radiusGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        radiusGO.transform.SetParent(parentInfluenceSphere);
        radiusGO.transform.localPosition = Vector3.zero;

        float diameter = radius * 2;
        Vector3 parentScale = parentInfluenceSphere.localScale;

        Vector3 localScale = new Vector3
        (
            diameter / parentScale.x,
            diameter / parentScale.y,
            diameter / parentScale.z
        );

        radiusGO.transform.localScale = localScale;

        radiusGO.GetComponent<Renderer>().material = material;

        SceneVisibilityManager.instance.DisablePicking(radiusGO, false); // makes it so this object can't be selected caues it gets in the way

        return radiusGO; 
    }

    public void DespawnInfluence(Influence influence)
    {
        switch (influence)
        {
            case SemanticInfluence semanticInfluence:
                DespawnSemanticInfluence(semanticInfluence);
                break;
            case ColorInfluence visualInfluence:
                DespawnColorInfluence(visualInfluence);
                break;
        }
    }

    private void DespawnSemanticInfluence(SemanticInfluence influence)
    {
        // debugging
        Debug.Log("influence despawned");

        GameObject instance = spawnedSemanticInfluences[influence];
        Destroy(instance);
        spawnedSemanticInfluences.Remove(influence);
    }

    private void DespawnColorInfluence(ColorInfluence influence)
    {
        // debugging
        Debug.Log("visual influence despawned");

        GameObject instance = spawnedColorInfluences[influence];
        Destroy(instance);
        spawnedColorInfluences.Remove(influence);
    }

    public void ToggleInfluenceVisibility(bool visible)
    {
        foreach(GameObject semanticInfluenceGO in spawnedSemanticInfluences.Values)
        {
            semanticInfluenceGO.SetActive(visible);
        }

        foreach (GameObject colorInfluenceGO in spawnedColorInfluences.Values)
        {
            colorInfluenceGO.SetActive(visible);
        }
    }

    public void ToggleInfluenceRadiusVisibility(bool visible)
    {
        foreach (GameObject semanticInfluenceGO in spawnedSemanticInfluences.Values)
        {
            semanticInfluenceGO.transform.GetChild(0).gameObject.SetActive(visible);
        }

        foreach (GameObject colorInfluenceGO in spawnedColorInfluences.Values)
        {
            colorInfluenceGO.transform.GetChild(0).gameObject.SetActive(visible);
        }
    }
}
