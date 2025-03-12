using UnityEngine;

public class PathOnMesh : MonoBehaviour
{
    public Vector3 startPoint;
    public Mesh mesh;
    public float stepSize;

    public Vector3[] path;

    void Start()
    {
        //1) we raycast down and get the triangle index as well as the surface normal (we can also calculate it, not sure yet)
        //2) get step vector
        //3) get triangle points
        //4) 
    }

    // return the corners of the mesh triangle at position
    public Vector3[] GetTrianglePoints(int index)
    {
        return null;
    }

    // returns step direction
    public Vector3 GetStepVector(Vector3 normal)
    {
        return Vector3.up;
    }

    // finds if and with which triangle edge the step vector intersects
    public int FindIntersection (Vector3 edge, Vector3 step)
    {
        return 0;
    }


}
