using UnityEngine;

public class TestController : MonoBehaviour
{
    public PathDisplayer pathDisplayer;
    void Start()
    {
        Vector3[] testPoints = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 1, 0),
            new Vector3(4, 0, 2),
            new Vector3(6, 2, 1),
            new Vector3(8, 0, 0)
        };

        pathDisplayer.DisplayPathPoints(testPoints);
        pathDisplayer.DisplayPathLines(testPoints);
    }

}
