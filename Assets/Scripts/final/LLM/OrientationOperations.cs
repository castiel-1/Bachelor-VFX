using UnityEngine;

public static class OrientationOperations
{
    public static Vector3[] CalculateLineDirections(Vector3[] pathPoints)
    {
        Vector3[] lineDirections = new Vector3[pathPoints.Length];

        for (int i = 0; i < pathPoints.Length; i++)
        {
            Vector3 direction;

            // start node
            if (i == 0)
            {
                direction = (pathPoints[i + 1] - pathPoints[i]).normalized;
            }
            // end node
            else if (i == pathPoints.Length - 1)
            {
                direction = (pathPoints[i] - pathPoints[i - 1]).normalized;
            }
            else
            {
                Vector3 directionBefore = (pathPoints[i] - pathPoints[i - 1]).normalized;
                Vector3 directionAfter = (pathPoints[i + 1] - pathPoints[i]).normalized;
                direction = (directionBefore + directionAfter).normalized;
            }

            lineDirections[i] = direction;
        }

        return lineDirections;
    }
}
