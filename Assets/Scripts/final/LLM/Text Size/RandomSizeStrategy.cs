using UnityEngine;

public class RandomSizeStrategy : ITextSizeStrategy
{
    public float[] GetTextSizes(int numLetters)
    {
        float min = RuntimeSettingsData.textSizeMin;
        float max = RuntimeSettingsData.textSizeMax;

        float[] sizes = new float[numLetters];

        for(int i = 0; i < numLetters; i++)
        {
            sizes[i] = UnityEngine.Random.Range(min, max + 1);
        }

        return sizes;
    }
}
