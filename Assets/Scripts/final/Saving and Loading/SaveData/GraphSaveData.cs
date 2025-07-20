using System;
using System.Collections.Generic;

[Serializable]
public class GraphSaveData
{
    public int NodeID;
    public List<NodeSaveData> Nodes;
    public List<PathSaveData> Paths;

}
