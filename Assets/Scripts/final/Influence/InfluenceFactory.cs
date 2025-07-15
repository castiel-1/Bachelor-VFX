using UnityEngine;
using UnityEngine.UIElements;

public static class InfluenceFactory
{
    public static Influence CreateSemanticInfluence(Vector3 position, float radius, string promptModifier, string name, GameObject prefab)
    {
        // debugging
        Debug.Log("create influence called in factory method");

        // use standard sphere if no gameObject chosen
        if(prefab == null)
        {
            prefab = Resources.Load<GameObject>("Prefabs/finalPrefabs/influenceP");
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

        Influence influence = new Influence(position, radius, promptModifier, name, prefab);

        return influence;
    }
}
