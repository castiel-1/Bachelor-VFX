using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PathHandleDisplayer : MonoBehaviour
{
    public GameObject handlePrefab;
    private Dictionary<Handle, GameObject> handleOnPathObjectsDict = new();

    private void OnEnable()
    {
        HandleOperations.OnHandleOnPathCreated += SpawnHandleOnPath;
        HandleOperations.OnHandleOnPathDestroyed += DespawnHandle;
        HandleOperations.OnToggleAllHandles += ToggleHandles;
    }

    private void OnDisable()
    {
        HandleOperations.OnHandleOnPathCreated -= SpawnHandleOnPath;
        HandleOperations.OnHandleOnPathDestroyed -= DespawnHandle;
        HandleOperations.OnToggleAllHandles -= ToggleHandles;
    }

    public void SpawnHandleOnPath(Handle handle, Path path)
    {
        // debugging
        Debug.Log("Spawn Handle on path called");

        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity);
        handleGO.AddComponent<HandleOnPathComponent>().Initialize(path, handle);

        handleOnPathObjectsDict[handle] = handleGO;
    }

    public void DespawnHandle(Handle handle)
    {
        // debugging
        Debug.Log("Despawn Handle called");

        GameObject handleGO = handleOnPathObjectsDict[handle];
        Destroy(handleGO);

        handleOnPathObjectsDict.Remove(handle);
    }

    public void DeactivateAllHandles()
    {
        // debugging
        Debug.Log("disabling all handles");
        // debugging
        Debug.Log("HandleDisplayer enabled! Instance ID: " + GetInstanceID());

        foreach (GameObject handleObject in handleOnPathObjectsDict.Values)
        {
            Debug.Log("disabling handle: " +  handleObject.name);
            handleObject.SetActive(false);
        }
    }

    public void ActivateAllHandles()
    {
        // debugging
        Debug.Log("enabling all handles");

        foreach (GameObject handleObject in handleOnPathObjectsDict.Values)
        {
            handleObject.SetActive(true);
        }
    }

    public void ToggleHandles(bool active)
    {
        if (active)
        {
            foreach (GameObject handle in handleOnPathObjectsDict.Values)
            {
                handle.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject handle in handleOnPathObjectsDict.Values)
            {
                handle.SetActive(false);
            }
        }
    }
}
