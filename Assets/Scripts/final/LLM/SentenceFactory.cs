using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public static class SentenceFactory
{
    public static (Sentence, LetterStruct[]) CreateSentence(string text, int startIndex, List<Vector3> letterPositions, float[] sizes, Vector3 normal, Vector3[] lineDirections, Color[] colors)
    {
        if(lineDirections == null)
        {
            // debugging
            Debug.Log("line directions null");

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
                lineDirection = lineDirections[i],
                normal = normal,
                size = sizes[i],
                color = colors[i]
            };
            Debug.Log("normal: " + normal);

            letters[i] = letter;
        }

        Sentence sentence = new Sentence(text, startIndex);

        return (sentence, letters);
    }
}
