using System;

public class Sentence
{
    public int StartIndex { get; } // where the sentence starts in the graphics buffer
    public string Text { get; }
    public Sentence(string text, int startIndex)
    {
        StartIndex = startIndex;
        Text = text;
    }
}
