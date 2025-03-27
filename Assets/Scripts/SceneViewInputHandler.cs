#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneViewInputHandler : MonoBehaviour
{
    public PathOnMesh pathScript;
    private int stepCount = 0;

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
            Debug.Log("Space pressed in Scene View!");

                pathScript.Visualize(stepCount);
                stepCount++; 

            e.Use();
        }
    }
}
#endif
