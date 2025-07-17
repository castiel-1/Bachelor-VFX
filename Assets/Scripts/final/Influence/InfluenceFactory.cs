using UnityEngine;
using UnityEngine.UIElements;

public static class InfluenceFactory
{
    public static SemanticInfluence CreateSemanticInfluence(Vector3 position, float radius, string promptModifier, string name, GameObject prefab)
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

        SemanticInfluence influence = new SemanticInfluence(position, radius, promptModifier, name, prefab);

        return influence;
    }

    public static VisualInfluence CreateVisualInfluence(Vector3 position, string name, float radius, Color color, GameObject prefab)
    {
        // use standard sphere if no gameObject chosen
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("Prefabs/finalPrefabs/visualInfluenceP");
        }

        VisualInfluence visualInfluence = new VisualInfluence(position, radius, color, name, prefab);

        return visualInfluence;
    }
}
