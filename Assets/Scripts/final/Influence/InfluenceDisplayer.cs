using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class InfluenceDisplayer : MonoBehaviour
{
    public static InfluenceDisplayer Instance { get; private set; }

    private Dictionary<SemanticInfluence, GameObject> spawnedSemanticInfluences = new();
    private Dictionary<VisualInfluence, GameObject> spawnedVisualInfluences = new();

    private Transform semanticParentTransform;
    private Transform visualParentTransform;

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
        visualParentTransform = visualGO.transform;
    }

    private void OnEnable()
    {
        InfluenceManager.OnSemanticInfluenceCreated += SpawnSemanticInfluence;
        InfluenceManager.OnVisualInfluenceCreated += SpawnVisualInfluence;

        InfluenceManager.OnSemanticInfluenceDeleted += DespawnSemanticInfluence;
        InfluenceManager.OnVisualInfluenceDeleted += DespawnVisualInfluence;
    }

    private void OnDisable()
    {
        InfluenceManager.OnSemanticInfluenceCreated -= SpawnSemanticInfluence;
        InfluenceManager.OnSemanticInfluenceDeleted -= DespawnSemanticInfluence;
    }

    public void SpawnSemanticInfluence(SemanticInfluence influence)
    {
        // debugging
        Debug.Log("influence spawned");

        GameObject instance = Instantiate(influence.Prefab, influence.Position, Quaternion.identity);
        instance.name = influence.Name;
        instance.transform.SetParent(semanticParentTransform);

        // spawn radius
        Material radiusMaterial = Resources.Load<Material>("Materials/semanticInfluenceRadiusM");
        GameObject radiusGO = SpawnRadius(influence.Radius, radiusMaterial, instance.transform);

        spawnedSemanticInfluences.Add(influence, instance);
    }

    public void SpawnVisualInfluence(VisualInfluence influence)
    {
        GameObject instance = Instantiate(influence.Prefab, influence.Position, Quaternion.identity);
        instance.name = influence.Name;
        instance.transform.SetParent(visualParentTransform);

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

        spawnedVisualInfluences.Add(influence, instance);
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

    public void DespawnSemanticInfluence(SemanticInfluence influence)
    {
        // debugging
        Debug.Log("influence despawned");

        GameObject instance = spawnedSemanticInfluences[influence];
        Destroy(instance);
        spawnedSemanticInfluences.Remove(influence);
    }

    public void DespawnVisualInfluence(VisualInfluence influence)
    {
        // debugging
        Debug.Log("visual influence despawned");

        GameObject instance = spawnedVisualInfluences[influence];
        Destroy(instance);
        spawnedVisualInfluences.Remove(influence);
    }

    public void ToggleSemanticInfluenceVisibility(bool visible)
    {
        foreach(GameObject influenceGO in spawnedSemanticInfluences.Values)
        {
            influenceGO.SetActive(visible);
        }
    }

    public void ToggleSemanticInfluenceRadiusVisibility(bool visible)
    {
        foreach (GameObject influenceGO in spawnedSemanticInfluences.Values)
        {
            influenceGO.transform.GetChild(0).gameObject.SetActive(visible);
        }
    }
}
