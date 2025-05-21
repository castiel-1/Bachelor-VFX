using UnityEngine;

public class ClickListenerSetup : MonoBehaviour
{
    public TestController testController;

    void Awake()
    {
#if UNITY_EDITOR
        ClickManager.testController = testController;
#endif
    }
}