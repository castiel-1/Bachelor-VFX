using UnityEngine;
using UnityEngine.VFX;


/* 

IF IN SCENE
- provides the blueprint for letterStructs

METHODS
- SetUpBuffer(): sets up buffer
- UpdateBuffer(LetterStruct[]): updates the buffer with changes to all structs in letterStruct[]
- GetIndex(char): returns findex of char to be used in flipbook
- SetLetterCount(count): sets letterCount so we know how many letters we want to display <-- this is strictly necessary!!

 */

// struct for each letter holding information for displaying it

[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct LetterStruct
{
    public int fIndex;
    public Vector3 position;
    public Vector3 normal;
    public Vector3 lineDirection;
    public float size;
}

public class Buffer : MonoBehaviour
{
    VisualEffect VisualEffect { get; set; }
    int NumLetters { get; set; }

    private GraphicsBuffer graphicsBuffer;

    // create graphics buffer that can hold information for all letters
    public void SetUpBuffer()
    {
        //DEBUG
        Debug.Log("buffer is set up with number of letterStructs: " + NumLetters);

        graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, NumLetters,
            System.Runtime.InteropServices.Marshal.SizeOf(typeof(LetterStruct)));

        VisualEffect.SetGraphicsBuffer("LetterBuffer", graphicsBuffer);
    }


    // updates buffer to display any changes made to letterStructs
    public void UpdateBuffer(LetterStruct[] letterStructsToUpdate, int startIndex, int length)
    {
        //DEBUG
        if (graphicsBuffer == null)
        {
            Debug.Log("Graphics buffer is null");
        }
        if (VisualEffect == null)
        {
            Debug.Log("visual effect is null");
        }
        Debug.Log("update buffer called");

        graphicsBuffer.SetData(letterStructsToUpdate, 0, startIndex, length);

        Debug.Log("reloading buffer");
        VisualEffect.Reinit();
    }

    private void OnDestroy()
    {
        graphicsBuffer.Release();
    }
}
