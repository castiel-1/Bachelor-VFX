using NUnit.Framework;
using UnityEngine;
using static PathCalculator;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UIElements;
using System.Collections;
using System.Threading.Tasks;

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

    public LLMManager llmManager;
    
    void Start()
    {
        TestLLMManger();
    }

    public async void TestLLMManger()
    {
        string prompt = "Write a sentence. Your answer should only be this sentence.";
        string promptModifier = "The sentence is influenced to 50% by 'dragon' and 10% by 'sad'";

        string answer = await llmManager.PromptLLM(prompt, promptModifier);

        Debug.Log("llm reply: " + answer);
    }

    public void TestGraph()
    {
        GameObject graphObject = new GameObject("Graph");
        Graph graph = graphObject.AddComponent<Graph>();
        graph.Initialize(0);

        Node startNode = graph.CreateNode(Vector3.zero);
        Node endNode = graph.CreateNode(new Vector3(2, 2, 2));
        
        graph.CreatePath(startNode, endNode, 10);
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
