using UnityEngine;

public interface IPathCreationStrategy 
{
    public void HandleClick(RaycastHit hitInfo, Graph graph, int numPathPoints);
}
