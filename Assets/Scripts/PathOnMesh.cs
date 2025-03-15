using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathOnMesh : MonoBehaviour
{
    /* THE PLAN (never goes wrong)
    
      1) we pick a point on the mesh from the dictionary
      2) calculate normal vector with triangle corners
      3) calculate step vector
      4) make step
      5) if(intersection with next triangle)
            find intersection, remember how much of step is done, get corners of new triangle and go to 2)
         else 
            place point according to step, go to 4)

      */

    public Mesh mesh;
    public Vector3 stepDirection;
    public float stepSize;
    public GraphicsInfoBuffer buffer;

    public Vector3[] path;

    private MeshManager meshManager;
    private Dictionary<(Vector3, Vector3), List<Vector3>> triangleDict = new Dictionary<(Vector3, Vector3), List<Vector3>>();

    void Start()
    {
        // set up path array
        int numLetters = buffer.text.Length;
        path = new Vector3[numLetters];

        // get triangleDictionary
        triangleDict = meshManager.GetNeighbouringTrianglesDict();

        var corners = GetRandomTriangleOnMesh();
        Vector3 normal = CalculateNormal(corners.Item1, corners.Item2, corners.Item3);
        Vector3 step = GetStepVector(stepDirection, normal, stepSize);
    }

    // calculate one path
    public Vector3[] CreatePath()
    {
        return path;
    }

    // used to get a random start triangle
    public (Vector3, Vector3, Vector3) GetRandomTriangleOnMesh()
    {
        int rand = Random.Range(0, triangleDict.Keys.Count);
        var key = triangleDict.Keys.ElementAt(rand);
        Vector3 value = triangleDict[key][0];
        return (key.Item1, key.Item2, value);
    }

    // calculate normal vector of triangle
    public Vector3 CalculateNormal (Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;

        Vector3 normal = Vector3.Cross(ab, ac);

        return normal;
    }

    // returns step vector parallel to triangle and with stepsize as length
    public Vector3 GetStepVector(Vector3 direction, Vector3 normal, float stepSize)
    {
        Vector3 projected = direction - (Vector3.Dot(direction, normal) * normal);
        Vector3 step = projected.normalized * stepSize;
        return step;
    }

    // finds if and with which triangle edge the step vector intersects
    public int FindIntersection (Vector3 edge, Vector3 step)
    {
        return 0;
    }


}
