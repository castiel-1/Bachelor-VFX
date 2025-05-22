using UnityEngine;

public class Sentence
{
    public int StartIndex { get; }
    public int Length { get; }
    public string Text { get; }
    public Sentence(int startIndex, int length, string text)
    {
        StartIndex = startIndex;
        Length = length;
        Text = text;
    }
}
