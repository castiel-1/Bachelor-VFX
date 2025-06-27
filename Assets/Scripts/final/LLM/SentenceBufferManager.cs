using System.Collections.Generic;
using UnityEngine;

public class SentenceBufferManager : MonoBehaviour
{
    public static SentenceBufferManager instance;

    public Buffer buffer;

    private Dictionary<Sentence, LetterStruct[]> sentenceStructDict = new();
    private int currentBufferIndex = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        HandleOperations.OnSplineUpdated += UpdateSentence;
    }
    private void OnDisable()
    {
        HandleOperations.OnSplineUpdated -= UpdateSentence;
    }

    // same size every letter
    public Sentence AddSentence(string text, Vector3[] letterPositions, float size, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        float[] sizes = new float[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            sizes[i] = size;
        }

        Sentence sentence = AddSentence(text, letterPositions, sizes, normals = null, lineDirections = null);

        return sentence;
    }

    // different sizes per letter
    public Sentence AddSentence(string text, Vector3[] letterPositions, float[] sizes, Vector3[] normals = null, Vector3[] lineDirections = null)
    {
        var (sentence, letters) = SentenceFactory.CreateSentence(text, currentBufferIndex, letterPositions, sizes, normals, lineDirections);
        sentenceStructDict.Add(sentence, letters);

        currentBufferIndex += sentence.Text.Length;

        buffer.AddSentenceToBuffer(letters, sentence);

        return sentence;
    }

    public void DeleteSentence(Sentence sentence)
    {
        sentenceStructDict.Remove(sentence);
        buffer.DeleteSentenceFromBuffer(sentence);
    }

    public void UpdateSentence(Path path)
    {
        // debugging
        Debug.Log("update sentence called");

        Sentence sentence = path.Sentence;
        var letterStructs = sentenceStructDict[sentence];

        for(int i = 0; i < letterStructs.Length; i++)
        {
            letterStructs[i].position = path.pathPoints[i];
        }

        buffer.AddSentenceToBuffer(letterStructs, sentence);
    }
}
