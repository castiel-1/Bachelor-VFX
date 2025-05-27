using UnityEngine;
using UnityEngine.UIElements;

public static class SentenceFactory
{
    // with just one size
    public static (Sentence, LetterStruct[]) CreateSentence(string text, int startIndex, Vector3[] letterPositions, float size, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        float[] sizes = new float[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            sizes[i] = size;
        }

        return CreateSentence(text, startIndex, letterPositions, sizes, normals, lineDirections);   
    }

    // with multiplpe sizes
    public static (Sentence, LetterStruct[]) CreateSentence(string text, int startIndex, Vector3[] letterPositions, float[] sizes, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        if(normals == null)
        {
            normals = new Vector3[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                normals[i] = Vector3.up;
            }
        }

        if(lineDirections == null)
        {
            lineDirections = new Vector3[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                lineDirections[i] = Vector3.up;
            }
        }

        LetterStruct[] letters = new LetterStruct[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            LetterStruct letter = new LetterStruct()
            {
                fIndex = CharSet.Instance.GetCharIndexInSet(text[i]),
                position = letterPositions[i],
                size = sizes[i]
            };

            letters[i] = letter;
        }

        Sentence sentence = new Sentence(text, startIndex);

        return (sentence, letters);
    }
}
