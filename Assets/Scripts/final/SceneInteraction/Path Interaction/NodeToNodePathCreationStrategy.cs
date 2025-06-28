using UnityEngine;

public class NodeToNodePathCreationStrategy : IPathCreationStrategy
{
    private Node startNode;
    private Node endNode;

    private PathCreationTool pathCreationTool;

    public NodeToNodePathCreationStrategy(PathCreationTool tool)
    {
        pathCreationTool = tool;
    }

    public void HandleClick(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.gameObject;
        NodeComponent nodeComponent = hitObject.GetComponent<NodeComponent>();

        // if we haven't clicked on a node we continue waiting
        if (hitObject == null || nodeComponent == null)
        {
            return;
        }

        Node currentNode = nodeComponent.Node;
        Graph graph = nodeComponent.Graph;

        if (startNode == null)
        {
            startNode = currentNode;
            //debugging
            Debug.Log("start node has been selected: " + startNode);
        }
        else if(currentNode != startNode)
        {
            endNode = currentNode;
            // debugging
            Debug.Log("end node has been selected: " + endNode);

          
            GraphOperations.CreatePath(graph, startNode, endNode);

            // debugging
            Debug.Log("new path has been created");

            pathCreationTool.StopInteraction();

            startNode = null;
            endNode = null;
        }


    }
}
