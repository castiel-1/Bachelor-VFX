using UnityEngine;

public class HandleComponent : MonoBehaviour
{
    public Path Path { get; private set; }

    public void Initialize(Path path)
    {
        Path = path;
    }

}
