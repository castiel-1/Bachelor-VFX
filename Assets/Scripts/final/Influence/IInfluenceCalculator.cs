using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IInfluenceCalculator
{
    public List<float> CalculateInfluenceStrengths(Vector3[] letterPositions, List<Influence> influence);
}
