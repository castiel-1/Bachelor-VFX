using Esper.ESave;
using UnityEngine;
using System.Collections.Generic;

public static class InfluenceSaveAndLoad 
{
    public static void SaveSemanticInfluences(List<SemanticInfluence> semanticInfluences, string semanticInfluencesSaveKey, SaveFile saveFile)
    {
        List<SemanticInfluenceSaveData> semanticInfluenceSaveDataList = new();

        foreach(SemanticInfluence semanticInfluence in semanticInfluences)
        {
            SemanticInfluenceSaveData semanticInfluenceSaveData = new SemanticInfluenceSaveData();

            semanticInfluenceSaveData.name = semanticInfluence.Name;
            semanticInfluenceSaveData.position = semanticInfluence.Position;
            semanticInfluenceSaveData.radius = semanticInfluence.Radius;
            semanticInfluenceSaveData.prefabName = semanticInfluence.Prefab.name;
            semanticInfluenceSaveData.promptModifier = semanticInfluence.PromptModifier;

            semanticInfluenceSaveDataList.Add(semanticInfluenceSaveData);
        }

        saveFile.AddOrUpdateData(semanticInfluencesSaveKey, semanticInfluenceSaveDataList);
        saveFile.Save();

        // debugging
        Debug.Log("semantic influences saved");
    }

    public static void SaveColorInfluences(List<ColorInfluence> colorInfluences, string colorInfluenceSaveKey, SaveFile saveFile)
    {
        List<ColorInfluenceSaveData> colorInfluenceSaveDataList = new();

        foreach(ColorInfluence colorInfluence in colorInfluences)
        {
            ColorInfluenceSaveData colorInfluenceSaveData = new ColorInfluenceSaveData();

            colorInfluenceSaveData.name = colorInfluence.Name;
            colorInfluenceSaveData.position = colorInfluence.Position;
            colorInfluenceSaveData.radius = colorInfluence.Radius;
            colorInfluenceSaveData.prefabName = colorInfluence.Prefab.name;
            colorInfluenceSaveData.color = colorInfluence.Color.ToSavable();

            colorInfluenceSaveDataList.Add(colorInfluenceSaveData);
        }

        saveFile.AddOrUpdateData(colorInfluenceSaveKey , colorInfluenceSaveDataList);
        saveFile.Save();

        // debugging
        Debug.Log("color influences saved");
    }

    public static void LoadSemanticInfluences(string semanticInfluenceSaveKey, SaveFile saveFile)
    {
        List<SemanticInfluenceSaveData> semanticInfluences = saveFile.GetData<List<SemanticInfluenceSaveData>>(semanticInfluenceSaveKey);

        foreach(SemanticInfluenceSaveData semanticInfluence in semanticInfluences)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/finalsPrefabs/{semanticInfluence.prefabName}");

            InfluenceManager.Instance.AddSemanticInfluence(semanticInfluence.name, semanticInfluence.position, semanticInfluence.radius, prefab, semanticInfluence.promptModifier);
        }

        // debugging
        Debug.Log("semantic influences loaded");
    }

    public static void LoadColorInfluences(string colorInfluenceSaveKey, SaveFile saveFile)
    {
        List<ColorInfluenceSaveData> colorInfluences = saveFile.GetData<List<ColorInfluenceSaveData>>(colorInfluenceSaveKey);

        foreach(ColorInfluenceSaveData colorInfluence in colorInfluences)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/finalsPrefabs/{colorInfluence.prefabName}");

            InfluenceManager.Instance.AddColorInfluence(colorInfluence.name, colorInfluence.position, colorInfluence.radius, prefab, colorInfluence.color.colorValue);
        }

        // debugging
        Debug.Log("color influences loaded");
    }
}
