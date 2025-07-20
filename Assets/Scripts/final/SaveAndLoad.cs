#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveAndLoad
{
    [MenuItem("Tools/Save Scene to Prefab ")]
    public static void SaveSceneAsPrefab()
    {
        // Create a temporary parent to hold the scene objects
        GameObject root = new GameObject("SceneSnapshot");

        GameObject userCreation = GameObject.Find("UserCreation");

        foreach (Transform child in userCreation.transform)
        {
            CloneHierarchy(child, root.transform);
        }

        // Save as prefab
        string timestamp = System.DateTime.Now.ToString("MMddyyyy_HHmmss");
        string path = $"Assets/SavedScenes/SceneSnapshot_{timestamp}.prefab";

        PrefabUtility.SaveAsPrefabAsset(root, path);
        Debug.Log("Saved scene to prefab: " + path);

        // Clean up
        EditorApplication.delayCall += () =>
        {
            Object.DestroyImmediate(root);
        };
    }

    // Recursive function to clone a transform and all its children
    private static void CloneHierarchy(Transform source, Transform parent)
    {
        // Instantiate a copy of the source GameObject
        GameObject copy = Object.Instantiate(source.gameObject);
        copy.name = source.name;

        // Set its parent in the new hierarchy
        copy.transform.SetParent(parent);

        // Reset local transform to keep same relative position/rotation/scale
        copy.transform.localPosition = source.localPosition;
        copy.transform.localRotation = source.localRotation;
        copy.transform.localScale = source.localScale;

        // Recurse for all children
        foreach (Transform child in source)
        {
            CloneHierarchy(child, copy.transform);
        }
    }
}
#endif