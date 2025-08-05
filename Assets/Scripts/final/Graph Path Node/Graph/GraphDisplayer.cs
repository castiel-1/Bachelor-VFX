using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class GraphDisplayer : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject nodePrefab;
    public GameObject handlePrefab;

    public Dictionary<Path, List<GameObject>> pathPointObjects = new();

    private Graph graph;

    private Dictionary<Path, GameObject> pathObjects = new();
    private Dictionary<Node, GameObject> nodeObjects = new();
    private Dictionary<Handle, GameObject> handleOnNodeObjects = new();
    private Dictionary<Handle, GameObject> handleOnPathObjects = new();

    private void Awake()
    {
        graph = GetComponent<Graph>();
    }

    private void OnEnable()
    {
        graph.OnNodeCreated += SpawnNode;
        graph.OnNodeDeleted += DespawnNode;
        graph.OnPathPointsAdded += SpawnPathPoints;
        graph.OnPathDeleted += DespawnPathPoints;

        HandleManager.OnHandleOnNodeCreated += SpawnHandleOnNode;
        HandleManager.OnHandleOnNodeDestroyed += DespawnHandleOnNode;
        HandleManager.OnHandleOnPathCreated += SpawnHandleOnPath;
        HandleManager.OnHandleOnPathDestroyed += DespawnHandleOnNode;
        HandleManager.OnSplineUpdated += UpdatePathPoints;
        HandleManager.OnToggleAllHandles += ToggleHandles;

        HandleManager.OnHandleOnPathRecreated += MoveSpawnedHandleOnPath;

        GraphOperations.OnToggleNodes += ToggleNodes;
        GraphOperations.OnTogglePathPoints += TogglePathPoints;
    }
    private void OnDisable()
    {
        graph.OnNodeCreated -= SpawnNode;
        graph.OnNodeDeleted -= DespawnNode;
        graph.OnPathPointsAdded -= SpawnPathPoints;
        graph.OnPathDeleted -= DespawnPathPoints;

        HandleManager.OnHandleOnNodeDestroyed -= DespawnHandleOnNode;
        HandleManager.OnHandleOnNodeCreated -= SpawnHandleOnNode;
        HandleManager.OnHandleOnPathCreated -= SpawnHandleOnPath;
        HandleManager.OnHandleOnPathDestroyed -= DespawnHandleOnNode;
        HandleManager.OnSplineUpdated -= UpdatePathPoints;
        HandleManager.OnToggleAllHandles -= ToggleHandles;

        HandleManager.OnHandleOnPathRecreated -= MoveSpawnedHandleOnPath;

        GraphOperations.OnToggleNodes -= ToggleNodes;
        GraphOperations.OnTogglePathPoints -= TogglePathPoints;
    }

    public void SpawnPathPoints(Path path, Graph graph)
    {
        // debugging
        Debug.Log("spawning " + path.pathPoints.Count + " path points");

        GameObject pathParent = new GameObject("Path_" + path.StartNode.ID + "_" + path.EndNode.ID);
        pathParent.transform.parent = transform;
        pathParent.SetActive(false);

        pathObjects[path] = pathParent;

        List<GameObject> pointObjects = new();

        for(int i = 0; i < path.pathPoints.Count; i++)
        {
            GameObject nextPoint = Instantiate(pointPrefab, path.pathPoints[i], Quaternion.identity, pathParent.transform);
            nextPoint.AddComponent<PathPointComponent>().Initialize(path, graph, i);
            SceneVisibilityManager.instance.DisablePicking(nextPoint, false);

            // make points textsize : 1,5 so they are nicely sized
            float textSize;
            if(RuntimeSettingsData.textSizeMode == RuntimeSettingsData.TextSizeMode.OneSize)
            {
                textSize = RuntimeSettingsData.textSizeMin;
            }
            else
            {
                textSize = (RuntimeSettingsData.textSizeMax - RuntimeSettingsData.textSizeMin) / 2f;
            }
            float sphereRadius = textSize / 1.5f;

            // debugging
            Debug.Log("point prefab sphere radius: " + sphereRadius);

            nextPoint.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);

            pointObjects.Add(nextPoint);
        }

        pathPointObjects.Add(path, pointObjects);
    }

    public void DespawnPathPoints(Path path)
    {
        // destroy path point gameobjects
        foreach (GameObject pointGO in pathPointObjects[path])
        {
            Destroy(pointGO);
        }

        // destroy path gameobject
        Destroy(pathObjects[path]);

        // remove from the dictionaries
        pathObjects.Remove(path);
        pathPointObjects.Remove(path);
    }
    public void SpawnHandleOnNode(Handle handle, Node node)
    {
        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity, transform);
        handleGO.name = "nodeHandle_" + node.ID;
        var handleComponent = handleGO.AddComponent<HandleOnNodeComponent>();
        handleComponent.Initialize(node, handle);

        handleComponent.OnHandleMoved += UpdateNode;

        handleGO.SetActive(false);

        handleOnNodeObjects[handle] = handleGO;
    }

    public void DespawnHandleOnNode(Handle handle)
    {
        GameObject handleGO = handleOnNodeObjects[handle];
        HandleOnNodeComponent handleComponent = handleGO.GetComponent<HandleOnNodeComponent>();
        handleComponent.OnHandleMoved -= UpdateNode;

        Destroy(handleGO);
        handleOnNodeObjects.Remove(handle);
    }
    
    public void SpawnHandleOnPath(Handle handle, Path path)
    {
        // debugging
        Debug.Log("Spawn Handle on path called");

        GameObject handleGO = Instantiate(handlePrefab, handle.Position, Quaternion.identity, pathObjects[path].transform);
        handleGO.name = "pathHandle_";
        handleGO.AddComponent<HandleOnPathComponent>().Initialize(path, handle);

        handleOnPathObjects[handle] = handleGO;
    }

    public void DespawnHandleOnPath(Handle handle)
    {
        // debugging
        Debug.Log("Despawn Handle called");

        GameObject handleGO = handleOnPathObjects[handle];
        Destroy(handleGO);

        handleOnPathObjects.Remove(handle);
    }

    public void MoveSpawnedHandleOnPath(Handle handle, Vector3 position, Path path)
    {
        // debugging
        Debug.Log("handle moved after recreation");

        GameObject handleGO = handleOnPathObjects[handle];
        handleGO.transform.SetPositionAndRotation(position, Quaternion.identity);

        // update spline because with inactive GO the component won't trigger the update
        HandleManager.Instance.UpdateSpline(path);
    }

    public void SpawnNode(Node node, Graph graph)
    {
        GameObject nodeGO = Instantiate(nodePrefab, node.Position, Quaternion.identity, transform);
        nodeGO.name = "Node_" + node.ID;
        nodeGO.AddComponent<NodeComponent>().Initialize(node, graph);
        nodeGO.SetActive(false);

        SceneVisibilityManager.instance.DisablePicking(nodeGO, false); // makes it so this object can't be selected 

        nodeObjects.Add(node, nodeGO);
    }

    public void DespawnNode(Node node, Path path)
    {
        Destroy(nodeObjects[node]);

        nodeObjects.Remove(node);

        HandleManager.Instance.DeleteHandleOnNode(node, path);
    }

    public void UpdatePathPoints(Path path)
    {
        List<GameObject> pathPoints = pathPointObjects[path];

        for(int i = 0;  i < pathPoints.Count; i++)
        {
            pathPoints[i].transform.position = path.pathPoints[i];
        }
    }

    public void UpdateNode(Node node)
    {
        GameObject nodeGO = nodeObjects[node];
        nodeGO.transform.position = node.Position;
    }

    public void ToggleHandles(bool active)
    {
        foreach(GameObject handleOnNode in handleOnNodeObjects.Values)
        {
            handleOnNode.SetActive(active);
        }

        foreach (GameObject handleOnPath in handleOnPathObjects.Values)
        {
            handleOnPath.SetActive(active);
        }
    }

    public void ToggleNodes(bool active)
    {
        foreach(GameObject nodeGO in nodeObjects.Values)
        {
            nodeGO.SetActive(active);
        }
    }

    public void TogglePathPoints(bool active)
    {
        foreach(Path path in pathPointObjects.Keys)
        {
            pathObjects[path].SetActive(active);
        }
    }
}
