#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    public static TargetCursor Instance { get; private set; }

    public float maxDistance = 1000f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public Vector3 GetSurfacePoint()
    {
        Camera cam = SceneView.lastActiveSceneView.camera;

        if(cam == null)
        {
            Debug.Log("you need to be in scene view, otherwise camera is null");
        }

        Vector3 cursorPosition = GetCursorPosition();

        Vector3 direction = (cursorPosition - cam.transform.position).normalized;

        Ray ray = new Ray(cam.transform.position, direction);

        // debugging
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan, 2f);

        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // debugging
            Debug.Log("raycast hit in point: " + hit.point);

            return hit.point;
        }

        Debug.Log("nothing hit with raycast, no meshes close enough");
        return Vector3.zero;
    }

    public Vector3 GetCursorPosition()
    {
        Transform cursorPrefabTransform = transform.GetChild(0);

        // debugging
        Debug.Log("target cursor position requested: " + cursorPrefabTransform.position);

        return cursorPrefabTransform.position;
    }
}
