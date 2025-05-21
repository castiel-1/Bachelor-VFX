using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TriangleIndexDisplayer : MonoBehaviour
{
    public MeshHandler meshHandler;
    public TextMeshPro textBox;

    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();
    private int numLettersForDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get sortedTrianglesDict
        sortedTrianglesDict = meshHandler.GetSortedTrianglesDict();

        // number of letters for display
        numLettersForDisplay = sortedTrianglesDict.Keys.Count();

        // debugging
        Debug.Log("number of triangles: " + numLettersForDisplay);

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
