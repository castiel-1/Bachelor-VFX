using UnityEngine;

public class Influence
{
    public string Name { get; }
    public Vector3 Position { get; set; }
    public float Radius { get; }
    public GameObject Prefab { get; }

    public Influence(string name, Vector3 position, float radius, GameObject prefab)
    {
        Name = name;
        Position = position;
        Radius = radius;
        Prefab = prefab;
    }

}
