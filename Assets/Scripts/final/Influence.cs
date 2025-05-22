using UnityEngine;

public class Influence
{
    public Vector3 Position { get; }
    public int Radius { get; }
    public string PromptModifier { get; }

    public GameObject Prefab { get; }

    public Influence(Vector3 position, int radius, string promptModifier, GameObject prefab)
    {
        Position = position;
        Radius = radius;
        PromptModifier = promptModifier;
        Prefab = prefab;
    }
}
