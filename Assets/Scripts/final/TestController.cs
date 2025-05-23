using NUnit.Framework;
using UnityEngine;
using static PathCalculator;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UIElements;
using System.Collections;

public class TestController : MonoBehaviour
{

    public GraphDisplayer pathDisplayer;
    private Vector3[] testPoints = new Vector3[]
    {
            new Vector3(0, 0, 0),
            new Vector3(2, 1, 0),
            new Vector3(4, 0, 2),
            new Vector3(6, 2, 1),
            new Vector3(8, 0, 0)
    };

    private Node startNode;
    private Node endNode;

    public PathCalculator pathCalculator;
    public int numLetters = 5;
    private Vector3[] letterPositions;
    private PathInfo[] pointInfos;
    private List<Vector3> allPoints = new List<Vector3>();

    public TargetCursor cursor;
    public GameObject debugSphere;

    public InfluenceManager influenceManager;

    
    void Start()
    {
    }

    public void TestNode()
    {
        startNode = new Node(0, new Vector3(2, 1, 0));
        endNode = new Node(1, new Vector3(8, 0, 0));

        Debug.Log("startNode pos: " + startNode.Position);
        Debug.Log("endNode pos: " + endNode.Position);
    }

    public Vector3[] TestPathCalculator()
    {
        Debug.Log("test path calculator called");

        return pathCalculator.CalculateLetterPositions(numLetters);
    }

    public void TestCursor()
    {
        Vector3 rayCastHit = cursor.ScreenTo3D();

        Instantiate(debugSphere, rayCastHit, Quaternion.identity);
    }

    public void TestInfluence()
    {
        influenceManager.CreateInfluence(Vector3.zero, 1f, "test", debugSphere);
    }


}
