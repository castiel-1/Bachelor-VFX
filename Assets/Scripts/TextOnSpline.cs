using UnityEngine;

public class TextOnSpline : MonoBehaviour
{
    public GraphicsInfoBuffer graphicsBuffer;
    public CatmullRomSpline spline;

    private int letterCount;

    public void Start()
    {
        letterCount = graphicsBuffer.text.Length;
        Debug.Log("Letter count: " + letterCount);
        
    }

    public void OnSplineUpdated(Vector3[] splinePoints)
    {
        LetterStruct[] letterStructs = new LetterStruct[letterCount];

        if (splinePoints.Length < letterCount)
        {
            Debug.LogWarning("Not enough points on the spline for the letters.");
            return;
        }

        for (int i = 0; i < letterCount; i++)
        {

            int j = Mathf.FloorToInt((i / (float)(letterCount - 1)) * (splinePoints.Length - 1));

            letterStructs[i] = new LetterStruct
            {
                fIndex = graphicsBuffer.GetIndex(graphicsBuffer.text[i]),
                position = splinePoints[j],
                direction = Vector3.up,
                size = 0.5f,
            };

          
        }

        //DEBUG
        if(graphicsBuffer == null)
        {
            Debug.Log("Graphics buffer is null");
        }

        graphicsBuffer.UpdateBuffer(letterStructs);

        //DEBUG
        Debug.Log("update buffer called from textonspline");


    }
}
