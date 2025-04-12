using UnityEngine;

public class DisplayPath : MonoBehaviour
{
    public GraphicsInfoBuffer graphicsBuffer;
    public PathOnMesh pathOnMesh;

    private int letterCount;
    private PathOnMesh.PathInfo[] pathInfo;

    public void Start()
    {
        // set up pathInfo
        letterCount = graphicsBuffer.text.Length;
        pathInfo = new PathOnMesh.PathInfo[letterCount];

        // display path
        Debug.Log("displaying path now...");
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
                direction = pathInfo[i].normal,
                size = 0.5f,
            };

        }

    }
}

