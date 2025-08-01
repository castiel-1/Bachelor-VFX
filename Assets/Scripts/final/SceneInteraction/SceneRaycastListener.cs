using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class SceneRaycastListener
{
    private static Action<RaycastHit> OnHover;
    private static Func<RaycastHit, bool> OnLeftClick; // the bool is false when left click should not be used
    private static Action OnMiss;

    public static bool isListening = false; // expose to other scripts that the SceneRaycastListener is currently active

    // to avoid errors when exiting play mode
    static SceneRaycastListener()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredEditMode)
        {
            StopRaycastListener(); // Cleanup safely when exiting Play Mode
        }
    }

    public static void StartRaycastListener(Action<RaycastHit> onHover, Func<RaycastHit, bool> onLeftClick, Action onMiss)
    {
        // protection from subscribing more than once if we ever forget to unsubscribe
        if (!isListening)
        {
            SceneView.duringSceneGui += OnSceneGUI;

        }

        OnHover = onHover;
        OnLeftClick = onLeftClick;
        OnMiss = onMiss;
        isListening = true;
    }

    public static void StopRaycastListener()
    {
        // protection from unsubscribing more than once if we ever forget to subscribe
        if (isListening)
        {
            SceneView.duringSceneGui -= OnSceneGUI;

        }

        OnHover = null;
        OnLeftClick = null;
        OnMiss = null;
        isListening = false;
    }

    public static void OnSceneGUI(SceneView sceneView)
    {
        if (!isListening)
        {
            return;
        }

        Event e = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        // if the ray hits 
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            OnHover?.Invoke(hitInfo);

            // if a left click is detected
            if(e.type == EventType.MouseDown && e.button == 0)
            {
                bool useLeftClick = OnLeftClick.Invoke(hitInfo);

                if(useLeftClick)
                {
                    e.Use();
                }
            }
        }
        else
        {
            // if the ray doesn't hit
            OnMiss?.Invoke();
        }

        sceneView.Repaint();
    }
}
