using System;
using System.Collections.Generic;
using Esper.ESave.SavableObjects;

[Serializable]
public class PathSaveData
{
    public int startNodeID;
    public int endNodeID;
    public List<HandleSaveData> handles;
    public string sentenceText;
}
