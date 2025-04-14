using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayTriangleNumbers : MonoBehaviour
{
    public MeshManager meshManager;
    public TextMeshPro textBox;

    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();
    private int numLettersForDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get sortedTrianglesDict
        sortedTrianglesDict = meshManager.GetSortedTrianglesDict();

        // number of letters for display
        numLettersForDisplay = sortedTrianglesDict.Keys.Count();

        ShowTriangleNumbers();
    }

    public void ShowTriangleNumbers()
    {

        // for each triangle, calculate centroid and write down the index we want to show
        for (int i = 0; i < numLettersForDisplay; i++)
        {
            var corners = sortedTrianglesDict.Keys.ElementAt(i);
            Vector3 centroid = (corners.Item1 + corners.Item2 + corners.Item3) / 3f;

            string number = i.ToString();

            TMP_Text text = Instantiate(textBox, centroid, Quaternion.identity);
            text.text = number;
            text.fontSize = 5;

        }
    }
}
