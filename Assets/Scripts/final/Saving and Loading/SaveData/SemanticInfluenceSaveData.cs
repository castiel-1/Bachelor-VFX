using System;
using Esper.ESave.SavableObjects;

[Serializable]
public class SemanticInfluenceSaveData 
{
    public string name;
    public SavableVector position;
    public float radius;
    public string prefabName;
    public string promptModifier;
}
