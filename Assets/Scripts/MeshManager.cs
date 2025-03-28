using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class MeshManager : MonoBehaviour
{
    public Mesh mesh;

    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();
    private Vector3[] vertices;
    private int[] triangles;

    public void Awake()
    {
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        
        triangleDict = CreateNeighbouringTrianglesDict();
        sortedTrianglesDict = CreateSortedTrianglesDictionary();



        // Debugging
        Debug.Log("number of edges in dict: " + triangleDict.Keys.Count);

        // Debugging
        Debug.Log("mesh dictionary created");


        /*
        // Debugging

        foreach(var key in triangleDict.Keys)
        {

            GameObject lineObject = new GameObject("LineRendererObject");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            lineRenderer.startWidth = 0.005f;
            lineRenderer.endWidth = 0.005f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.positionCount = 2;

            lineRenderer.SetPosition(0, key.Item1);
            lineRenderer.SetPosition(1, key.Item2);

        }
        */


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

    // debugging mesh
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(Vector3 vertex in vertices)
        {
            Gizmos.DrawSphere(vertex, 0.02f);
        }
    }*/


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

    public Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> CreateSortedTrianglesDictionary()
    {

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner1 = vertices[triangles[i]];
            Vector3 corner2 = vertices[triangles[i + 1]];
            Vector3 corner3 = vertices[triangles[i + 2]];

            // the correct order to tell us if it's inward or outward facing
            (Vector3, Vector3, Vector3) orderedCorners = (corner1, corner2, corner3);

            // the sorted corners to make it so they can be looked up
            var sortedCorners = GetVertexOrder(corner1, corner2, corner3);

            sortedTrianglesDict.Add(sortedCorners, orderedCorners);
        }

        return sortedTrianglesDict;
    }

    public (Vector3, Vector3, Vector3) GetVertexOrder(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var orderedV1V2 = GetEdgeOrder(v1, v2);
        var orderedV1V3 = GetEdgeOrder(v1, v3);
        var orderedV2V3 = GetEdgeOrder(v2, v3);

        var first = orderedV1V2.Item1; 
        var second = orderedV1V2.Item2;

        if (GetEdgeOrder(second, v3).Item1 == second)
        {
            return (first, second, v3);
        }
        else
        {
            return (first, v3, second);
        }
    }

public Dictionary<(Vector3, Vector3), List<Vector3>> GetNeighbouringTrianglesDict()
    {
        // Debugging
        Debug.Log("triangleDict requested");

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

    public Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> GetSortedTrianglesDict()
    {
        return sortedTrianglesDict;
    }
}
