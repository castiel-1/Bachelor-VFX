using System;
using System.Threading.Tasks;
using LLMUnity;
using UnityEngine;

// calling this works as follows: call from an async method like this: 
// string x = await PromptLLM(...); 

public class LLMManager : MonoBehaviour
{
    public static LLMManager Instance { get; private set; }
    public LLMCharacter llm;

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

    public async Task<string> PromptLLM(string prompt)
    {
        string reply = await llm.Chat(prompt, null, null, false);
        return reply;
    }
}
