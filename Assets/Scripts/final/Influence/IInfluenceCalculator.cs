using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IInfluenceCalculator
{
    public List<float> CalculateInfluenceStrengths(Path path, List<Influence> influences);
}
