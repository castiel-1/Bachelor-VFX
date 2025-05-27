using UnityEngine;

public class CharSet : MonoBehaviour
{
    public static CharSet Instance { get; private set; }
    string Chars { get; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public CharSet(string chars)
    {
        Chars = chars;
    }

    public int GetCharIndexInSet(char c)
    {
        return Chars.IndexOf(c);
    }
}
