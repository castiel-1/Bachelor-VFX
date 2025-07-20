using UnityEngine;

public class ColorInfluence : Influence
{
    public Color Color { get; }

    public ColorInfluence(string name, Vector3 position, float radius, GameObject prefab, Color color)
        : base (name, position, radius, prefab)
    {
        Color = color;
    }
}
