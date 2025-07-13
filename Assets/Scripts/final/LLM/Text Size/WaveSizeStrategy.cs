using UnityEngine;

public class WaveSizeStrategy : ITextSizeStrategy
{
    public float[] GetTextSizes(int numLetters)
    {
        float min = RuntimeSettingsData.textSizeMin;
        float max = RuntimeSettingsData.textSizeMax;

        float[] sizes = new float[numLetters];

        for (int i = 0; i < numLetters; i++)
        {
            float step = (float)i / (numLetters - 1);
            float wave = Mathf.Sin(step * Mathf.PI);

            sizes[i] = Mathf.Lerp(min, max, wave);  
        }

        return sizes;
    }
}
