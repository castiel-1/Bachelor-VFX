using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HandleDisplayer : MonoBehaviour
{
    public GameObject handlePrefab;
    private Dictionary<Handle, GameObject> handleObjects = new();


    private void OnEnable()
    {
        HandleOperations.onHandleCreated += SpawnHandle;
        HandleOperations.onHandleDestroyed += DespawnHandle;
    }

    private void OnDisable()
    {
        HandleOperations.onHandleCreated -= SpawnHandle;
        HandleOperations.onHandleDestroyed -= DespawnHandle;
    }

    public void SpawnHandle(Handle handle, Path path)
    {
        // debugging
        Debug.Log("Spawn Handle called");

        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity);
        handleGO.AddComponent<HandleComponent>().Initialize(path, handle);

        handleObjects[handle] = handleGO;
    }

    public void DespawnHandle(Handle handle)
    {
        // debugging
        Debug.Log("Despawn Handle called");

        GameObject handleGO = handleObjects[handle];
        Destroy(handleGO);

        handleObjects.Remove(handle);
    }

    public void DeactivateAllHandles()
    {
        // debugging
        Debug.Log("disabling all handles");

        foreach(GameObject handleObject in handleObjects.Values)
        {
            handleObject.SetActive(false);
        }
    }

    public void ActivateAllHandles()
    {
        // debugging
        Debug.Log("enabling all handles");

        foreach (GameObject handleObject in handleObjects.Values)
        {
            handleObject.SetActive(true);
        }
    }

}
