using UnityEngine;

public class GrowingSizeStrategy : ITextSizeStrategy
{
    public float[] GetTextSizes(int numLetters)
    {
        float min = RuntimeSettingsData.textSizeMin;
        float max = RuntimeSettingsData.textSizeMax;

        float[] sizes = new float[numLetters];


        float increase = (max - min) / (numLetters - 1);

        for(int i = 0;  i < numLetters; i++)
        {
            sizes[i] = min + (i * increase);
        }
        
        return sizes;
    }
}
