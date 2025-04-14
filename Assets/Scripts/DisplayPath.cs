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
        letterCount = pathOnMesh.GetLetterCount();
        pathInfo = pathOnMesh.GetPathInfo();

        // Debugging
        Debug.Log("letterCount: " + letterCount);

        // give letterCount to buffer
        graphicsBuffer.SetLetterCount(letterCount);

        // set up buffer
        graphicsBuffer.SetUpBuffer();

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
                fIndex = graphicsBuffer.GetIndex(pathOnMesh.GetText()[i]),
                position = pathInfo[i].point + pathInfo[i].normal * 0.01f,
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

