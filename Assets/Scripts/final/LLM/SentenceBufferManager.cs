using System.Collections.Generic;
using UnityEngine;

public class SentenceBufferManager : MonoBehaviour
{
    public static SentenceBufferManager Instance { get; private set; }

    private Dictionary<Sentence, LetterStruct[]> sentenceStructDict = new();
    private int currentBufferIndex = 0;
    private Vector3 maxSize = Vector3.zero;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        HandleOperations.OnSplineUpdated += UpdateSentence;
        GraphOperations.OnPathDeleted += DeleteSentence;
    }
    private void OnDisable()
    {
        HandleOperations.OnSplineUpdated -= UpdateSentence;
    }

    public Sentence AddSentence(string text, List<Vector3> letterPositions, float[] sizes, Vector3[] normals, Vector3[] lineDirections, Color[] colors)
    {

        // debugging
        Debug.Log("add sentence in sentencebuffermanager called");
        Debug.Log("size: " + sizes[0]);

        var (sentence, letters) = SentenceFactory.CreateSentence(text, currentBufferIndex, letterPositions, sizes, normals, lineDirections, colors);
        sentenceStructDict.Add(sentence, letters);

        currentBufferIndex += sentence.Text.Length;

        Vector3 size = CalculateMaxAndMinSize(letters);

        Buffer.Instance.AddSentenceToBuffer(letters, sentence, size);

        return sentence;
    }

    public void DeleteSentence(Sentence sentence)
    {
        sentenceStructDict.Remove(sentence);
        Buffer.Instance.DeleteSentenceFromBuffer(sentence);
    }

    public void UpdateSentence(Path path)
    {
        Sentence sentence = path.Sentence;
        var letterStructs = sentenceStructDict[sentence];

        Debug.Log("num letterstructs: " + letterStructs.Length + ", num pathPoints: " + path.pathPoints.Count);

        for(int i = 0; i < letterStructs.Length; i++)
        {
            letterStructs[i].position = path.pathPoints[i];
        }

        Vector3 size = CalculateMaxAndMinSize(letterStructs);

        Buffer.Instance.AddSentenceToBuffer(letterStructs, sentence, size);
    }

    // calculation for bounding box adjustment
    private Vector3 CalculateMaxAndMinSize(LetterStruct[] letterStructs)
    {
        foreach(LetterStruct letter in letterStructs)
        {
            Vector3 absolutePosition = new Vector3
            (
                Mathf.Abs(letter.position.x),
                Mathf.Abs(letter.position.y),
                Mathf.Abs(letter.position.z)
            );

            maxSize = Vector3.Max(maxSize, absolutePosition);
        }

        return maxSize * 2 + (Vector3.one * 0.5f); // size from center from -max to +max with padding
    }
}
