using UnityEngine;

public static class TextSizeStrategyFactory
{
    public static ITextSizeStrategy CreateTextSizeStrategy()
    {
        switch(RuntimeSettingsData.textSizeMode)
        {
            case RuntimeSettingsData.TextSizeMode.OneSize: return new OneSizeStrategy();
            case RuntimeSettingsData.TextSizeMode.Wave: return new WaveSizeStrategy();
            case RuntimeSettingsData.TextSizeMode.Growing: return new GrowingSizeStrategy();
            case RuntimeSettingsData.TextSizeMode.Shrinking: return new ShrinkingSizeStrategy();
            case RuntimeSettingsData.TextSizeMode.Random: return new RandomSizeStrategy();
            default: return new OneSizeStrategy();
        }
    }
}
