#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneViewInputHandler : MonoBehaviour
{
    public PathOnMesh pathScript;
    private int stepCount = 0;
    private GameObject previousLineObject;
    private GameObject previousLineObject2;
    bool previousHadIntersection = false;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            if (previousLineObject != null)
            {
                DestroyImmediate(previousLineObject);
            }
            if (previousLineObject2 != null)
            {
                DestroyImmediate(previousLineObject2);
            }

            // Create a new GameObject for the triangle visualization
            previousLineObject = new GameObject("TriangleRenderer");
            LineRenderer lineRenderer2 = previousLineObject.AddComponent<LineRenderer>();

            previousLineObject2 = new GameObject("TriangleRenderer");
            LineRenderer lineRenderer3 = previousLineObject2.AddComponent<LineRenderer>();

            Debug.Log("Space pressed in Scene View!");
            Debug.Log("previousHadIntersection: " + previousHadIntersection);

            previousHadIntersection = pathScript.Visualize(stepCount, lineRenderer2, lineRenderer3, previousHadIntersection);
            stepCount++;

            e.Use();
        }
    }
}
#endif
