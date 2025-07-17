using UnityEngine;

public class VisualInfluence : Influence
{
    public Color Color { get; }


    public VisualInfluence(string name, Vector3 position, float radius, GameObject prefab, Color color)
        : base (name, position, radius, prefab)
    {
        Color = color;
    }
}
