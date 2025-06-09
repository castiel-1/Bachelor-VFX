using UnityEngine;

public class NodeToNodePathCreationStrategy : IPathCreationStrategy
{
    private Node startNode;
    private Node endNode;

    public void HandleClick(RaycastHit hitInfo, Graph graph, int numPathPoints)
    {
        GameObject hitObject = hitInfo.collider.gameObject;

        // if we haven't clicked on a node we continue waiting
        if (hitObject == null )
        {
            return;
        }

        Node currentNode = hitObject.GetComponent<NodeComponent>().Node;

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

            // TODO 
            // implement this so that we can uncomment the line below
            // graph.CreatePath(startNode, endNode, numPathPoints);

            // debugging
            Debug.Log("path has been created");

            PathCreationController.EndPathCreation();

            startNode = null;
            endNode = null;

        }


    }
}
