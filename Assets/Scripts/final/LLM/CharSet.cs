using UnityEngine;

public class CharSet : MonoBehaviour
{
    public static CharSet Instance { get; private set; }
    public string chars;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int GetCharIndexInSet(char c)
    {
        return chars.IndexOf(c);
    }
}
