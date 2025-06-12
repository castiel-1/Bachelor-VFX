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

    public Graph graph;
    public FullPromptBuilder fullPromptBuilder;

    public GraphManager graphManager;

    
    void Start()
    {
        TestGraphCreation();
    }

    public void TestGraphCreation()
    {
        Graph graph = graphManager.CreateGraph();
        Node startNode = GraphOperations.CreateNode(graph, Vector3.zero);
        Node endNode = GraphOperations.CreateNode(graph, Vector3.one);
        Node thirdNode = GraphOperations.CreateNode(graph, new Vector3(2, 1, 1));

        GraphOperations.CreatePath(graph, startNode, endNode, 20);
    }

    /*
    public void SimplePathSetup()
    {
        // Create some nodes
        Node nodeA = graph.CreateNode(new Vector3(0, 0, 0)); // ID 0
        Node nodeB = graph.CreateNode(new Vector3(1, 0, 0)); // ID 1
        Node nodeC = graph.CreateNode(new Vector3(2, 0, 0)); // ID 2
        Node nodeD = graph.CreateNode(new Vector3(3, 0, 0)); // ID 3
        Node nodeE = graph.CreateNode(new Vector3(4, 0, 0)); // ID 4

        // Create paths (A → B → D and A → C → D → E)
        graph.CreatePath(nodeA, nodeB, 5); // A → B
        graph.CreatePath(nodeB, nodeD, 5); // B → D
        graph.CreatePath(nodeA, nodeC, 5); // A → C
        graph.CreatePath(nodeC, nodeD, 5); // C → D
        graph.CreatePath(nodeD, nodeE, 5); // D → E
    }

    public void TestFullPromptGenerator()
    {
        int numPoints = 10;


        // Create some nodes
        Node nodeA = graph.CreateNode(new Vector3(0, 0, 0)); // ID 0
        Node nodeB = graph.CreateNode(new Vector3(1, 0, 0)); // ID 1
        Node nodeC = graph.CreateNode(new Vector3(2, 0, 0)); // ID 2
        Node nodeD = graph.CreateNode(new Vector3(3, 0, 0)); // ID 3
        Node nodeE = graph.CreateNode(new Vector3(4, 0, 0)); // ID 4

        // Create paths (A → B → D and A → C → D → E)
        Path p1 = graph.CreatePath(nodeA, nodeB, 5); // A → B
        p1.Sentence = new Sentence("test1", 1);
        Path p2 = graph.CreatePath(nodeB, nodeD, 5); // B → D
        p2.Sentence = new Sentence("test2", 1);
        Path p3 = graph.CreatePath(nodeA, nodeC, 5); // A → C
        p3.Sentence = new Sentence("test3", 1);
        Path p4  = graph.CreatePath(nodeC, nodeD, 5); // C → D
        p4.Sentence = new Sentence("test4", 1);
        Path path = graph.CreatePath(nodeD, nodeE, 5); // D → E

        int depth = 2;

        influenceManager.CreateInfluence(new Vector3(3, 0, 0), 1f, "Influence1", debugSphere);
        influenceManager.CreateInfluence(new Vector3(3, 0, 0), 2f, "Influence2", debugSphere);

        IInfluenceCalculator calculator = new CountBasedInfluenceCalculator();
        IInfluencePromptGenerator generator = new CountBasedInfluencePromptGenerator();

        string fullPrompt = fullPromptBuilder.BuildPrompt(numPoints, graph, path, depth, influenceManager, calculator, generator);

        Debug.Log("full prompt: " +  fullPrompt);   
    }
    */
    public void TestHistoryPromptGenerator()
    {
        // Create nodes
        Node nodeA = new Node(0, Vector3.zero);
        Node nodeB = new Node(1, Vector3.one);
        Node nodeC = new Node(2, Vector3.up);
        Node nodeD = new Node(3, Vector3.right);

        // Create paths with sentences
        Path pathAB = new Path(nodeA, nodeB, 0) { Sentence = new Sentence("There was a dragon.", 0) };
        Path pathBD = new Path(nodeB, nodeD, 0) { Sentence = new Sentence("I had green scales.", 1) };
        Path pathCD = new Path(nodeC, nodeD, 0) { Sentence = new Sentence("There was a mouse.", 2) };

        // Branch 1: A -> B -> D
        var branchABD = new List<Path> { pathAB, pathBD };

        // Branch 2: C -> D
        var branchCD = new List<Path> { pathCD };

        var allBranches = new List<List<Path>> { branchABD, branchCD };

        // Generate prompt
        string prompt = HistoryPromptGenerator.GenerateHistoryPrompt(allBranches);

        // Print the generated prompt
        Debug.Log("Generated Prompt:" + prompt);
    }
    /*
    public void TestBackwardsTraversal()
    {
        // Create some nodes
        Node nodeA = graph.CreateNode(new Vector3(0, 0, 0)); // ID 0
        Node nodeB = graph.CreateNode(new Vector3(1, 0, 0)); // ID 1
        Node nodeC = graph.CreateNode(new Vector3(2, 0, 0)); // ID 2
        Node nodeD = graph.CreateNode(new Vector3(3, 0, 0)); // ID 3
        Node nodeE = graph.CreateNode(new Vector3(4, 0, 0)); // ID 4

        // Create paths (A → B → D and A → C → D → E)
        graph.CreatePath(nodeA, nodeB, 5); // A → B
        graph.CreatePath(nodeB, nodeD, 5); // B → D
        graph.CreatePath(nodeA, nodeC, 5); // A → C
        graph.CreatePath(nodeC, nodeD, 5); // C → D
        graph.CreatePath(nodeD, nodeE, 5); // D → E

        // Pick a target node and depth
        Node targetNode = nodeE;
        int depth = 3;

        // Perform the backwards traversal
        List<List<Path>> branches = graph.GetAllPreviousPaths(targetNode, depth);

        // Print out each branch
        Debug.Log($"Found {branches.Count} branches leading to node {targetNode.ID}:");
        for (int i = 0; i < branches.Count; i++)
        {
            string branchDesc = $"Branch {i + 1}: ";
            foreach (Path path in branches[i])
            {
                branchDesc += $"{path.StartNode.ID} → {path.EndNode.ID}, ";
            }
            Debug.Log(branchDesc.TrimEnd(',', ' '));
        }
    }
    */
    public void TestInfluenceCalculator()
    {
        Influence influence = new Influence(Vector3.zero, 1f, "sad", debugSphere);
        Influence influence1 = new Influence(Vector3.zero, 1f, "dragon", debugSphere);
        List<Influence> influences = new List<Influence>() { influence, influence1 };

        Vector3[] points = new Vector3[]
        {
            new Vector3(0.9f, 0, 0),
            new Vector3(0, 0.5f, 0.5f),
            new Vector3(3, 3, 3)
        };

        IInfluenceCalculator countBasedInfluenceCalculator = new CountBasedInfluenceCalculator();
        List<float> strengths = countBasedInfluenceCalculator.CalculateInfluenceStrengths(points, influences);

        Debug.Log("strength: " + strengths[0]);
        Debug.Log("strength: " + strengths[1]);

        IInfluencePromptGenerator countBasedInfluencePromptGenerator = new CountBasedInfluencePromptGenerator();
        string prompt = countBasedInfluencePromptGenerator.GenerateInfluencePrompt(influences, strengths);

        Debug.Log("prompt: " + prompt);

    }

    public async void TestLLMManger()
    {
        string prompt = "Write a sentence. Your answer should only be this sentence.";
        string promptModifier = "The sentence is influenced to 50% by 'dragon' and 10% by 'sad'";

        string answer = await llmManager.PromptLLM(prompt, promptModifier);

        Debug.Log("llm reply: " + answer);
    }

    /*public void TestGraph()
    {
        GameObject graphObject = new GameObject("Graph");
        Graph graph = graphObject.AddComponent<Graph>();
        graph.Initialize(0);

        Node startNode = graph.CreateNode(Vector3.zero);
        Node endNode = graph.CreateNode(new Vector3(2, 2, 2));
        
        graph.CreatePath(startNode, endNode, 10);
    }
    */
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
        Vector3 rayCastHit = cursor.GetSurfacePoint();

        Instantiate(debugSphere, rayCastHit, Quaternion.identity);
    }

    public void TestInfluence()
    {
        influenceManager.CreateInfluence(Vector3.zero, 1f, "test", debugSphere);
    }


}
