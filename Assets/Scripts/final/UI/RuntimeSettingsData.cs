using UnityEngine;

public static class RuntimeSettingsData
{
    public enum TextSizeMode { OneSize, Random, Growing, Shrinking, Wave }

    public static TextSizeMode textSizeMode;

    public static bool onSurface;

    // one size text will set the min value to the desired size
    public static float textSizeMin = 0.1f;
    public static float textSizeMax = 0.5f;

    public static int numberOfWordsMin = 3;
    public static int numberOfWordsMax = 4;

    public static int historyDepth = 3;

    // colour
    public static Color uninfluencedTextColor = Color.white;

    // orientation
    public static bool faceCameraPlane;
    public static Vector3 rotation = Vector3.up;
}
