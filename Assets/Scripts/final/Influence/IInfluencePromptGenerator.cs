using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public interface IInfluencePromptGenerator
{
    public string GenerateInfluencePrompt(List<Influence> influences, List<float> influenceStrengths);
}
