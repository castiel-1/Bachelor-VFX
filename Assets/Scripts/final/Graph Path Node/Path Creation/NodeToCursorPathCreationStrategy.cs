using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeToCursorPathCreationStrategy : IPathCreationStrategy
{
    private Node startNode;
    private Graph graph;
    private int numberOfPathPoints;
    private bool waitingForCursorConfirmation = false;
    private PathCreationTool pathCreationTool;
    public bool IsAwaitingCursorConfirmation => waitingForCursorConfirmation;

    public NodeToCursorPathCreationStrategy(PathCreationTool tool)
    {
        pathCreationTool = tool;
    }

    public void HandleClick(RaycastHit hitInfo, int numPathPoints)
    {
        numberOfPathPoints = numPathPoints;

        TargetCursor cursor = TargetCursor.Instance;

        GameObject hitObject = hitInfo.collider.gameObject;
        NodeComponent nodeComponent = hitObject.GetComponent<NodeComponent>();

        // if we haven't clicked on a node we continue waiting
        if (hitObject == null || nodeComponent == null)
        {
            return;
        }

        Node currentNode = nodeComponent.Node;
        graph = nodeComponent.Graph;

        if (startNode == null)
        {
            startNode = currentNode;
            //debugging
            Debug.Log("start node has been selected: " + startNode);

            // make button appear that asks for confirmation
            RuntimeInteractionData.AskForCursorPositionConfirmation = true;

            // tell the controller that we are waiting for the cursor confirmation so we stop using left clicks in the scene
            waitingForCursorConfirmation = true;
        }
    }

    public void HandleCursorPositionConfirmation()
    {
        TargetCursor cursor = TargetCursor.Instance;

        Vector3 cursorPosition = cursor.GetCursorPosition();

        Node endNode = GraphOperations.CreateNode(graph, cursorPosition);

        // create a path
        GraphOperations.CreatePath(graph, startNode, endNode, numberOfPathPoints);

        // debugging
        Debug.Log("new path with " + numberOfPathPoints + " points has been created");

        pathCreationTool.StopInteraction();

        RuntimeInteractionData.AskForCursorPositionConfirmation = false;
        startNode = null;
        graph = null;
        waitingForCursorConfirmation = false;
    }
}
