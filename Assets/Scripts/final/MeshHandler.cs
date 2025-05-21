using System;
using System.Collections.Generic;
using UnityEngine;

// When ordering the triangle, the rule is, we return the order (1, 2, 3) and one edge would then be (1, 2) and calculated as 2 - 1

// When interacting with neighbhouring triangles dict, the keys can be found by using GetEdgeOrder 

public class MeshHandler : MonoBehaviour
{
    public GameObject meshHolder;
    private Mesh mesh;

    private Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)> sortedTrianglesDict = new Dictionary<(Vector3, Vector3, Vector3), (Vector3, Vector3, Vector3)>();
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();
    private Vector3[] vertices;
    private int[] triangles;

    public void Awake()
    {
        // get mesh
        mesh = meshHolder.GetComponent<MeshFilter>().mesh;

        // get transforms for mesh
        Transform transform = meshHolder.transform;

        // get vertices and triangles list
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        // apply transform to vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }

        // create dictionaries
        triangleDict = CreateNeighbouringTrianglesDict();
        sortedTrianglesDict = CreateSortedTrianglesDictionary();

        //debugging
        Debug.Log("dictionaries created");
    }

    // Debugging
    private void OnDrawGizmos()
    {
        /*
        if (sortedTrianglesDict != null)
        {
            for (int j = 0; j < sortedTrianglesDict.Keys.Count; j++)
            {
                var corners = sortedTrianglesDict.Keys.ElementAt(j);

                Vector3 centroid = (corners.Item1 + corners.Item2 + corners.Item3) / 3f;

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(centroid, 0.01f);
            }

        }*/
    }

    public Dictionary<(Vector3, Vector3), List<Vector3>> CreateNeighbouringTrianglesDict()
    {

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner1 = vertices[triangles[i]];
            Vector3 corner2 = vertices[triangles[i + 1]];
            Vector3 corner3 = vertices[triangles[i + 2]];

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

    public (Vector3, Vector3) GetEdgeOrder(Vector3 v1, Vector3 v2)
    {

        List<Vector3> vectors = new List<Vector3> { v1, v2 };
        List<Vector3> orderedVectors = Order(vectors);

        return (orderedVectors[0], orderedVectors[1]);
    }

    public (Vector3, Vector3, Vector3) GetCornerOrder(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        List<Vector3> vectors = new List<Vector3> { v1, v2, v3 };
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

    // returns last corner of neighbhouring triangle
    public Vector3 GetNeighbouringTriangleLastCorner((Vector3, Vector3) edge, Vector3 corner)
    {
        // order edge correctly
        var orderedEdge = GetEdgeOrder(edge.Item1, edge.Item2);

        List<Vector3> corners = new List<Vector3>(triangleDict[orderedEdge]);

        corners.Remove(corner);

        Vector3 lastCorner = corners[0];

        return lastCorner;
    }

    // returns order of edge based on triangle
    public (Vector3, Vector3) GetEdgeOrderOnTriangle(Vector3 edgeStart, Vector3 edgeEnd, Vector3 lastCorner)
    {
        // get correct order of corners
        var cornerKey = GetCornerOrder(edgeStart, edgeEnd, lastCorner);
        var orderedCorners = sortedTrianglesDict[cornerKey];
        Vector3[] orderedCornersArray = new Vector3[] { orderedCorners.Item1, orderedCorners.Item2, orderedCorners.Item3 };

        // find correct order of edge (we start at 1 because we need to do modulo to wrap around the list)
        for (int i = 1; i < 4; i++)
        {
            Vector3 cornerStart = orderedCornersArray[i - 1];
            Vector3 cornerEnd = orderedCornersArray[i % 3];

            if ((cornerStart == edgeStart && cornerEnd == edgeEnd) || (cornerStart == edgeEnd && cornerEnd == edgeStart))
            {
                return (cornerStart, cornerEnd);
            }

        }

        return (edgeStart, edgeEnd);
    }

    // returns neighbouring triangle
    public (Vector3, Vector3, Vector3) GetNeighbouringTriangle((Vector3, Vector3) edge, Vector3 corner)
    {
        // order edge correctly
        var orderedEdge = GetEdgeOrder(edge.Item1, edge.Item2);

        List<Vector3> corners = new List<Vector3>(triangleDict[orderedEdge]);

        corners.Remove(corner);

        Vector3 lastCorner = corners[0];

        // Debugging
        // technically not needed but better to order it anyways - remove later
        var correctOrder = GetCornerOrder(edge.Item1, edge.Item2, lastCorner);

        return correctOrder;

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
