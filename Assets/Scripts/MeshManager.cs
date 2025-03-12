using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MeshManager : MonoBehaviour
{

    public Mesh mesh;

    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();
    private Vector3[] vertices;
    private int[] triangles;

    public void Start()
    {
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        triangleDict = CreateNeighbouringTrianglesDict();

        //Debugging

        /*
        foreach (var entry in triangleDict)
        {
            (Vector3 v1, Vector3 v2) = entry.Key;
            List<Vector3> neighbors = entry.Value;

            string neighborStr = string.Join(", ", neighbors);
            Debug.Log($"Edge: ({v1} -> {v2}) | Adjacent Vertices: {neighborStr}");
        } 

        Debug.Log("number of edges: " + triangleDict.Keys.Count);
        */
    }

    public Dictionary<(Vector3, Vector3), List<Vector3>> CreateNeighbouringTrianglesDict()
    {

        for (int i = 0; i < triangles.Length; i+=3)
        {
            Vector3 corner1 = vertices[triangles[i]];
            Vector3 corner2 = vertices[triangles[i+1]];
            Vector3 corner3 = vertices[triangles[i+2]];

            AddEdge(corner1, corner2, corner3); 
            AddEdge(corner2, corner3, corner1);
            AddEdge(corner3, corner1, corner2);
        }

        return triangleDict;
    }

    public void AddEdge(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var edge = GetEdgeOrder(v1, v2);

        if (!triangleDict.ContainsKey(edge))
        {
            triangleDict.Add(edge, new List<Vector3>());
        }

        triangleDict[edge].Add(v3);

    }

    public (Vector3, Vector3) GetEdgeOrder(Vector3 v1, Vector3 v2)
    {
        if((v1.x < v2.x) || (v1.x == v2.x && v1.y < v2.y) || (v1.x == v2.x && v1.y == v2.y && v1.z < v2.z))
        {
            return (v1, v2);
        }
        else
        {
            return (v2, v1);
        }
    }

    public Dictionary<(Vector3, Vector3), List<Vector3>> GetNeighbouringTrianglesDict()
    {
        return triangleDict;
    }
    public int[] GetTriangles()
    {
        return triangles;
    }

    public Vector3[] GetVertices()
    {
        return vertices;
    }
}
