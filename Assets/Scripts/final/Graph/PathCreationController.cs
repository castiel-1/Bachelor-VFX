using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[InitializeOnLoad]
public static class PathCreationController
{
    public static bool IsCreatingPath { get; private set; } = false;
    private static GameObject hoveredObject;

    static PathCreationController()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public static void StartPathCreation()
    {
        IsCreatingPath = true;
    }

    public static void EndPathCreation()
    {
        IsCreatingPath = false;
        hoveredObject = null;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!IsCreatingPath)
        {
            return;
        }

        Event e = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            GameObject target = hitInfo.collider.gameObject;
            if(hitInfo.collider != null)
            {
                hoveredObject = target;
            }
            else
            {
                hoveredObject = null;
            }

            // on left mouse click
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                GameObject clicked = hitInfo.collider.gameObject;

                if(clicked != null)
                {
                    Debug.Log("hit object: " + clicked);
                }

                e.Use();
            }
        }
        else
        {
            hoveredObject = null;
        }

        if(hoveredObject  != null)
        {
            Handles.color = Color.red;
            Bounds bounds = hoveredObject.gameObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }

        sceneView.Repaint();
    }
}
