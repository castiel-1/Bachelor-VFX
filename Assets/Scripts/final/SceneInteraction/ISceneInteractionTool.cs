using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public interface ISceneInteractionTool
{
    public void StartInteraction();
    public void StopInteraction();
}