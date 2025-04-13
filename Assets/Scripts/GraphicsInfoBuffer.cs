using UnityEngine;
using UnityEngine.VFX;


/* 

IF IN SCENE
- provides the blueprint for letterStructs
- sets up the graphicsBuffer based on how many letters are supposed to be saved in it

METHODS
- UpdateBuffer(LetterStruct[]): updates the buffer with changes to all structs in letterStruct[]
- GetIndex(char): returns findex of char to be used in flipbook

VARIABLES
- text: holds the text to be displayed

 */

// struct for each letter holding information for displaying it
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct LetterStruct
{
    public int fIndex;
    public Vector3 position;
    public Vector3 lineDirection;
    public Vector3 normal;
    public float size;
}

public class GraphicsInfoBuffer : MonoBehaviour
{
    public string text = "";
    public VisualEffect visualEffect;
    public GraphicsBuffer graphicsBuffer;
    public string charSet;

    public void Awake()
    {
        SetUpBuffer();
    }


    // create graphics buffer that can hold information for all letters
    public void SetUpBuffer()
    {
        //DEBUG
        Debug.Log("buffer is set up");
        Debug.Log("Text for displaying:" + text);

        int letterCount = text.Length;
        graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, letterCount,
            System.Runtime.InteropServices.Marshal.SizeOf(typeof(LetterStruct)));

        visualEffect.SetGraphicsBuffer("LetterBuffer", graphicsBuffer);
    }


    // updates buffer to display any changes made to letterStructs
    public void UpdateBuffer(LetterStruct[] letterStructs)
    {
        graphicsBuffer.SetData(letterStructs);

        Debug.Log("reloading buffer");
        visualEffect.Reinit();
    }

    // get fIndex of letter for use with the flipbook
    public int GetIndex(char letter)
    {
        return charSet.IndexOf(letter);
    }

    private void OnDestroy()
    {
        graphicsBuffer.Release();
    }
}
