using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public static class PathCreationController
{
    public static bool IsCreatingPath { get; private set; } = false;
    private static IPathCreationStrategy selectedStrategy;
    private static GameObject hoveredObject;
    public static IPathCreationStrategy SelectedStrategy => selectedStrategy;

    static PathCreationController()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // use this to start path creation (with button in UI)
    public static void StartPathCreation(IPathCreationStrategy startegy)
    {
        selectedStrategy = startegy;
        IsCreatingPath = true;
    }

    // use this to end path creation (with button in UI)
    public static void EndPathCreation()
    {
        selectedStrategy = null;
        IsCreatingPath = false;
        hoveredObject = null;
    }

    // checks for left click in scene view and finds if a node has been hit via raycast
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
            if(hitInfo.collider != null)
            {
                hoveredObject = hitInfo.collider.gameObject;
            }
            else
            {
                hoveredObject = null;
            }

            // on left mouse click
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                // if we are waiting for the cursor position confirmation we stop using left click
                if (selectedStrategy is NodeToCursorPathCreationStrategy cursorStrategy && cursorStrategy.IsAwaitingCursorConfirmation)
                {
                    return;
                }

                int numberOfPathPoints = RandomizeNumberOfPathPoints(RuntimeSettingsData.numberOfLettersMin, RuntimeSettingsData.numberOfLettersMax);
                selectedStrategy.HandleClick(hitInfo, numberOfPathPoints);
                e.Use();
            }
        }
        else
        {
            hoveredObject = null;
        }

        if (hoveredObject != null)
        {
            Handles.color = Color.red;
            Bounds bounds = hoveredObject.gameObject.GetComponent<Collider>().bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }

        sceneView.Repaint();
    }

    private static int RandomizeNumberOfPathPoints(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}
