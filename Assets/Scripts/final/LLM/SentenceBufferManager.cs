using System.Collections.Generic;
using UnityEngine;

public class SentenceBufferManager : MonoBehaviour
{
    public Buffer buffer;

    private Dictionary<Sentence, LetterStruct[]> sentenceStructDict;
    private int currentBufferIndex = 0;

    // same size every letter
    public void AddSentence(string text, Vector3[] letterPositions, float size, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        float[] sizes = new float[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            sizes[i] = size;
        }

        AddSentence(text, letterPositions, sizes, normals = null, lineDirections = null);
    }

    // different sizes per letter
    public void AddSentence(string text, Vector3[] letterPositions, float[] sizes, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        var (sentence, letters) = SentenceFactory.CreateSentence(text, currentBufferIndex, letterPositions, sizes, normals, lineDirections);
        sentenceStructDict.Add(sentence, letters);

        currentBufferIndex += sentence.Text.Length;

        buffer.AddSentenceToBuffer(letters, sentence);
    }

    public void DeleteSentence(Sentence sentence)
    {
        sentenceStructDict.Remove(sentence);
        buffer.DeleteSentenceFromBuffer(sentence);
    }
}
