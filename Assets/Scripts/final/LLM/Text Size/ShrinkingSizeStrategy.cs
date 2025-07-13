using UnityEngine;

public class ShrinkingSizeStrategy : ITextSizeStrategy
{
    public float[] GetTextSizes(int numLetters)
    {
        float min = RuntimeSettingsData.textSizeMin;
        float max = RuntimeSettingsData.textSizeMax;

        float[] sizes = new float[numLetters];

        float decrease = (max - min) / (numLetters - 1);

        for (int i = 0; i < numLetters; i++)
        {
            sizes[i] = max - (i * decrease);
        }

        return sizes;
    }
}
