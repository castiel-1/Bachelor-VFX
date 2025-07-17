using UnityEngine;

public class VisualInfluence
{
    public Vector3 Position { get; }
    public float Radius { get; }
    public Color Color { get; }
    public string Name { get; }
    public GameObject Prefab { get; }

    public VisualInfluence(Vector3 position, float radius, Color color, string name, GameObject prefab)
    {
        Position = position;
        Radius = radius;
        Color = color;
        Name = name;
        Prefab = prefab;
    }
}
