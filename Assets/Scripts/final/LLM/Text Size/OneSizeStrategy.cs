using UnityEngine;

public class OneSizeStrategy : ITextSizeStrategy
{
    public float[] GetTextSizes(int numLetters)
    {
        float min = RuntimeSettingsData.textSizeMin; // one size text sets min value to desired size

        float[] sizes = new float[numLetters];

        for(int i = 0; i < numLetters; i++)
        {
            sizes[i] = min;
        }

        return sizes;
    }
}
