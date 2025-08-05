using UnityEngine;

public class HandleOnPathComponent : MonoBehaviour
{
    public Path Path { get; private set; }
    public Handle Handle { get; private set; }

    private Vector3 lastPosition;

    public void Initialize(Path path, Handle handle)
    {
        Path = path;
        Handle = handle;

        lastPosition = transform.position;
    }

    // detect if handle is moved
    private void Update()
    {
        // if position has changed
        if(transform.position != lastPosition)
        {
            lastPosition = transform.position;
            Handle.Position = transform.position;

            HandleManager.Instance.UpdateSpline(Path);
        }
    }

}
