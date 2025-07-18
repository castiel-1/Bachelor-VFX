using UnityEngine;
using Unity.RuntimeSceneSerialization;
using System.IO;
using UnityEngine.SceneManagement;

public static class SaveAndLoad
{
    public static string saveFileName = "scene.json";

    public static void SaveScene()
    {
        // debugging
        Debug.Log("scene saved");

        Scene currentScene = SceneManager.GetActiveScene();

        string json = SceneSerialization.SerializeScene(currentScene);

        string fullPath = System.IO.Path.Combine(Application.persistentDataPath, saveFileName);

        File.WriteAllText(fullPath, json);
    }

    public static void LoadScene()
    {
        // debugging
        Debug.Log("scene loaded");

        string fullPath = System.IO.Path.Combine(Application.persistentDataPath, saveFileName);

        string json = File.ReadAllText(fullPath);

        SceneSerialization.ImportScene(json);
    }
}
