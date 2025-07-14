using UnityEngine;
using UnityEngine.UIElements;

public static class InfluenceFactory
{
    public static Influence CreateInfluence(Vector3 position, float radius, string promptModifier, string name, GameObject prefab)
    {
        // use standard sphere if no gameObject chosen
        if(prefab == null)
        {
            prefab = Resources.Load<GameObject>("Prefabs/finalPrefabs/influenceP");
        }

        // use beginning of prompt if no name is chosen
        if(name == null && promptModifier != null)
        {
            string[] promptWords = promptModifier.Split(' ');
            name = promptWords[0];
        }

        Influence influence = new Influence(position, radius, promptModifier, name, prefab);

        return influence;
    }
}
