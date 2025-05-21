#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ClickManager
{
    public static TestController testController;
    static ClickManager()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            Debug.Log("Space key pressed in Scene View!");

            if (testController != null)
            {
                //debugging
                Debug.Log("key pressed, calling testcursor()");

                testController.TestCursor();
            }
            else
            {
                Debug.LogWarning("TestController not assigned!");
            }


            e.Use(); 
        }
    }
}
#endif
