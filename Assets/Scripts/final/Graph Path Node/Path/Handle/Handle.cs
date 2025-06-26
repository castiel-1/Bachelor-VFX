using UnityEngine;

public class Handle 
{
    public Vector3 Position { get; set; }
    public int Index { get; set; } // index of pathPoint at which the handle is placed

    public Handle(Vector3 position, int index)
    {
        Position = position;
        Index = index;
    }
}
