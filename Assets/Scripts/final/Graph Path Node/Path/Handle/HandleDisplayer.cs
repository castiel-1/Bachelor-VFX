using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HandleDisplayer : MonoBehaviour
{
    public GameObject handlePrefab;
    private Dictionary<Handle, GameObject> handleObjectsDict = new();

    private void OnEnable()
    {
        // debugging
        Debug.Log("HandleDisplayer enabled! Instance ID: " + GetInstanceID());
        // debugging
        Debug.Log("on enable in graph interaction");

        HandleOperations.onHandleCreated += SpawnHandle;
        HandleOperations.onHandleDestroyed += DespawnHandle;
    }

    private void OnDisable()
    {
        // debugging
        Debug.Log("on disable in graph interaction");
        HandleOperations.onHandleCreated -= SpawnHandle;
        HandleOperations.onHandleDestroyed -= DespawnHandle;
    }

    public void SpawnHandle(Handle handle, Path path)
    {
        // debugging
        Debug.Log("Spawn Handle called");

        // debugging
        Debug.Log("HandleDisplayer enabled! Instance ID: " + GetInstanceID());

        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity);
        handleGO.AddComponent<HandleComponent>().Initialize(path, handle);

        handleObjectsDict[handle] = handleGO;
        //debugging
        Debug.Log("there are " + handleObjectsDict.Values.Count + " handle objects");
    }

    public void DespawnHandle(Handle handle)
    {
        // debugging
        Debug.Log("Despawn Handle called");

        GameObject handleGO = handleObjectsDict[handle];
        Destroy(handleGO);

        handleObjectsDict.Remove(handle);
    }

    public void DeactivateAllHandles()
    {
        // debugging
        Debug.Log("disabling all handles");
        // debugging
        Debug.Log("HandleDisplayer enabled! Instance ID: " + GetInstanceID());

        foreach (GameObject handleObject in handleObjectsDict.Values)
        {
            Debug.Log("disabling handle: " +  handleObject.name);
            handleObject.SetActive(false);
        }
    }

    public void ActivateAllHandles()
    {
        // debugging
        Debug.Log("enabling all handles");

        foreach (GameObject handleObject in handleObjectsDict.Values)
        {
            handleObject.SetActive(true);
        }
    }

}
