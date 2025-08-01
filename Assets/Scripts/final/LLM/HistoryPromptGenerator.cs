using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public static class HistoryPromptGenerator
{
    public static string GenerateHistoryPrompt(List<List<Path>> allBranches)
    {
        if(allBranches.Count == 0)
        {
            return "";
        }

        // debugging
        Debug.Log("all branches count: " + allBranches.Count);

        string prompt = "The sentence should take into account the story so far: ";

        for(int i = 0; i < allBranches.Count; i++)
        {
            string branchText = "";
            var branch = allBranches[i];

            foreach(var path in branch)
            {
                branchText += path.Sentence.Text + " ";
            }

            if(i == allBranches.Count - 1)
            {
                prompt += "'" + branchText.Trim() + "'.";
            }
            else
            {
                prompt += "'" + branchText.Trim() + "' and ";
            }

        }

        return prompt;
    }
}
