using UnityEngine;

public class Influence
{
    public Vector3 Position { get; }
    public float Radius { get; }
    public string PromptModifier { get; }
    public string Name { get; }
    public GameObject Prefab { get; }

    public Influence(Vector3 position, float radius, string promptModifier, string name, GameObject prefab)
    {
        Position = position;
        Radius = radius;
        PromptModifier = promptModifier;
        Name = name;
        Prefab = prefab;
    }
}
