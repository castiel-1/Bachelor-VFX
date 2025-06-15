using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class SceneRaycastListener
{
    public static Action<RaycastHit> OnHover;
    public static Func<RaycastHit, bool> OnLeftClick; // the bool is false when left click should not be used
    public static Action OnMiss;

    public static bool isListening = false;

    static SceneRaycastListener()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public static void StartRaycastListener(Action<RaycastHit> onHover, Func<RaycastHit, bool> onLeftClick, Action onMiss)
    {
        OnHover = onHover;
        OnLeftClick = onLeftClick;
        OnMiss = onMiss;
        isListening = true;
    }

    public static void StopRaycastListener()
    {
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
