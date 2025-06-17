using System;
using UnityEngine;

public static class HandleOperations
{
    public static event Action<Handle, Path> onHandleCreated;
    public static event Action<Handle> onHandleDestroyed;

    public static Handle CreateHandle(Vector3 position, Path path)
    {
        Handle nextHandle = new Handle(position);

        path.Handles.Add(nextHandle);

        UpdateSpline(path);

        onHandleCreated?.Invoke(nextHandle, path);

        return nextHandle;
    }

    public static void DeleteHandle(Handle handle, Path path)
    {
        path.Handles.Remove(handle);

        UpdateSpline(path);

        onHandleDestroyed?.Invoke(handle);
    }

    public static void UpdateSpline(Path path)
    {
        // get the path handle belongs to, get all handles, recalculate
        Handle[] handles = path.Handles.ToArray();
        int numHandles = handles.Length;
    }
}
