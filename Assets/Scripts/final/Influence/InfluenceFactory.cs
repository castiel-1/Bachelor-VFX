using UnityEngine;
using UnityEngine.UIElements;

public static class InfluenceFactory
{
    public static SemanticInfluence CreateSemanticInfluence(string name, Vector3 position, float radius, GameObject prefab, string promptModifier)
    {
        // debugging
        Debug.Log("create semantic influence called in factory method");

        // use standard sphere if no gameObject chosen
        if(prefab == null)
        {
            prefab = Resources.Load<GameObject>("Prefabs/finalPrefabs/semanticInfluenceP");
        }

        // use beginning of prompt if no name is chosen
        if(name == "" && promptModifier != "")
        {
            // debugging
            Debug.Log("name is null and will be replaced by prompt mod");

            string[] promptWords = promptModifier.Split(' ');

            Debug.Log(promptWords[0]);

            name = promptWords[0];
        }

        SemanticInfluence influence = new SemanticInfluence(name, position, radius, prefab, promptModifier);

        return influence;
    }

    public static ColorInfluence CreateColorInfluence(string name, Vector3 position, float radius, GameObject prefab, Color color)
    {
        // use standard sphere if no gameObject chosen
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("Prefabs/finalPrefabs/visualInfluenceP");
        }

        ColorInfluence visualInfluence = new ColorInfluence(name, position, radius, prefab, color);

        return visualInfluence;
    }
}
