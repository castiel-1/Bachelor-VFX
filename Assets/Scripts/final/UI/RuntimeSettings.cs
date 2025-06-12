public static class RuntimeSettings
{
    public enum PathType { Bezier, PathOnMesh }
    public enum CreationMode { Manual, Auto }
    public enum TextSizeMode { OneSize, Random, Growing, Shrinking, Wave }

    public static PathType pathType;
    public static CreationMode creationMode;
    public static TextSizeMode textSizeMode;

    public static bool gravity;

    public static float textSize = 0.2f;
    public static float textSizeMin = 0.1f;
    public static float textSizeMax = 0.5f;

    public static int numberOfLettersMin = 10;
    public static int numberOfLettersMax = 200;
}
