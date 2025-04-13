using UnityEngine;

public class TextOnWall : MonoBehaviour
{
    public GraphicsInfoBuffer buffer;

    private int letterCount;

    void Start()
    {
        letterCount = buffer.text.Length;
        LetterStruct[] letterStructs = new LetterStruct[letterCount];

        for (int i = 0; i < letterCount; i++)
        {
            letterStructs[i] = new LetterStruct
            {
                fIndex = buffer.GetIndex(buffer.text[i]),
                position = Vector3.up,
                normal = Vector3.up,
                size = 0.3f,
            };

        }

        buffer.UpdateBuffer(letterStructs);
    }
}
