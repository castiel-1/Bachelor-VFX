using System;
using Esper.ESave.SavableObjects;
using UnityEngine;

[Serializable]
public class ColorInfluenceSaveData 
{
    public string name;
    public SavableVector position;
    public float radius;
    public string prefabName;
    public SavableVector color;
}
