using UnityEngine;

public interface IPathCreationStrategy 
{
    public void HandleClick(RaycastHit hitInfo);
}
