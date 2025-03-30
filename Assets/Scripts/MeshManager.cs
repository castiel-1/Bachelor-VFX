using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

// When ordering the triangle, the rule is, we return the order (1, 2, 3) and the edge would then be (1, 2) and calculated as 2 - 1

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

        /*
        // Debugging
        foreach (var key in triangleDict.Keys)
        {
            Debug.Log(key + ": " + triangleDict[key].Count);
        }*/

        /*
        // Debugging
        Debug.Log("number of edges in dict: " + triangleDict.Keys.Count);

        // Debugging
        Debug.Log("mesh dictionary created");


        // Debugging
        Debug.Log("SortedTrianglesDict: ");

        foreach(var key in sortedTrianglesDict.Keys)
        {
            Debug.Log(key + ": " + sortedTrianglesDict[key] + "\n");
        }*/

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
            var sortedCorners = GetCornerOrder(corner1, corner2, corner3);

            sortedTrianglesDict.Add(sortedCorners, orderedCorners);
        }

        return sortedTrianglesDict;
    }

    /*
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
    */

    public (Vector3, Vector3) GetEdgeOrder(Vector3 v1, Vector3 v2)
    {
        // Debugging
        Debug.Log("in get edge order");

        List<Vector3> vectors = new List<Vector3> {v1, v2};
        List<Vector3> orderedVectors = Order(vectors);

        // Debugging
        Debug.Log("leaving get edge order");

        return (orderedVectors[0], orderedVectors[1]);
    }

    public (Vector3, Vector3, Vector3) GetCornerOrder(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        List<Vector3> vectors = new List<Vector3> {v1, v2, v3};
        List<Vector3> orderedVectors = Order(vectors);

        return (orderedVectors[0], orderedVectors[1], orderedVectors[2]);
    }

    public List<Vector3> Order(List<Vector3> vectors)
    {
        vectors.Sort((a, b) => a.x != b.x ? b.x.CompareTo(a.x) :
                               a.y != b.y ? b.y.CompareTo(a.y) :
                                            b.z.CompareTo(a.z));

        return vectors;
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
