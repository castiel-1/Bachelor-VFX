using UnityEngine;

public class SemanticInfluence : Influence
{
    public string PromptModifier { get; }

    public SemanticInfluence(string name, Vector3 position, float radius, GameObject prefab, string promptModifier)
        : base (name, position, radius, prefab)
    {
        PromptModifier = promptModifier;
    }
}
