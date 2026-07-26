using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class DecisionResults
{
    public PlayerDecision playerDecision;
    public PlayerDecision correctDecision;
    public DecisionOutcome outcome;

    public bool wasCorrect;

    // can link to +salary here
    //public int moneyChange;

    public List<string> reasons = new List<string>();
}