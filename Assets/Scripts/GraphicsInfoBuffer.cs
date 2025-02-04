using UnityEngine;
using UnityEngine.VFX;

[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct LetterStruct
{
    public int fIndex;
    public Vector3 position;
    public Vector3 direction;
    public float size;
}

public class GraphicsInfoBuffer : MonoBehaviour
{
    public string text = "hello";
    public VisualEffect visualEffect;
    public GraphicsBuffer graphicsBuffer;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public string charSet;

    public void Awake()
    {
        SetUpBuffer();
    }

    public void SetUpBuffer()
    {
        //DEBUG
        Debug.Log("buffer is set up");

        int letterCount = text.Length;
        graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, letterCount,
            System.Runtime.InteropServices.Marshal.SizeOf(typeof(LetterStruct)));

        visualEffect.SetGraphicsBuffer("LetterBuffer", graphicsBuffer);

    }


    public void UpdateBuffer(LetterStruct[] letterStructs)
    {
        //DEBUG
        if (graphicsBuffer == null){
            Debug.Log("Graphics buffer is null");
        }
        if(visualEffect == null)
        {
            Debug.Log("visual effect is null");
        }
        Debug.Log("update buffer called");

        graphicsBuffer.SetData(letterStructs);

        Debug.Log("reloading buffer");
        visualEffect.Reinit();
    }
    public int GetIndex(char letter)
    {

        return charSet.IndexOf(letter);
    }

    private void OnDestroy()
    {
        graphicsBuffer.Release();
    }
}
