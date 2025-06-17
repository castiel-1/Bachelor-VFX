using UnityEngine;

public class PathPointComponent : MonoBehaviour
{
    public Path Path { get; private set; }
    public Graph Graph { get; private set; }

    public void Initialize(Path path, Graph graph)
    {
        Path = path;
        Graph = graph;
    }
}
