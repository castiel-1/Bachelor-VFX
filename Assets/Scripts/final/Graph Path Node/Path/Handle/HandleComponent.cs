using UnityEngine;

public class HandleComponent : MonoBehaviour
{
    public Path Path { get; private set; }
    public Handle Handle { get; private set; }

    public void Initialize(Path path, Handle handle)
    {
        Path = path;
        Handle = handle;
    }

}
