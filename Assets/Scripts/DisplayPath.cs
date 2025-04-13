using UnityEngine;

public class DisplayPath : MonoBehaviour
{
    public GraphicsInfoBuffer graphicsBuffer;
    public PathOnMesh pathOnMesh;

    private int letterCount;
    private PathOnMesh.PathInfo[] pathInfo;

    public void Start()
    {
        // Debugging
        Debug.Log("in Display path");

        // set up pathInfo
        letterCount = graphicsBuffer.text.Length;
        pathInfo = pathOnMesh.GetPathInfo();

        // Debugging
        Debug.Log("letterCount: " + letterCount);

        // display path
        ShowPath();
    }

    // display the path provided by PathOnMesh with GraphicsInfoBuffer
    public void ShowPath()
    {
        // set up letterStructs
        LetterStruct[] letterStructs = new LetterStruct[letterCount];

        for (int i = 0; i < letterCount; i++)
        {

            letterStructs[i] = new LetterStruct
            {
                fIndex = graphicsBuffer.GetIndex(graphicsBuffer.text[i]),
                position = pathInfo[i].point,
                normal = pathInfo[i].normal,
                lineDirection = pathInfo[i].lineDirection,
                size = 0.3f,
            };

            // Debugging
            Debug.Log("position: " + pathInfo[i].point);
            Debug.Log("normal: " + pathInfo[i].normal);
        }

        // update buffer
        graphicsBuffer.UpdateBuffer(letterStructs);

    }
}

