using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DocumentVerification documentVerification;

    public DecisionResults ProcessDecision(NPCData npc, PlayerDecision playerDecision)
    {
        DecisionResults result = new DecisionResults();

        result.playerDecision = playerDecision;

        if (npc == null)
        {
            result.wasCorrect = false;
            result.outcome = DecisionOutcome.Incorrect;
            result.reasons.Add("No npc datra");
            return result;
        }

        List<string> discrepancies = documentVerification.VerifyNPC(npc);

        result.correctDecision = DetermineCorrectDecision(npc, discrepancies);

        result.wasCorrect = playerDecision == result.correctDecision;

        CalculateOutcome(npc, result);

        return result;

    }

    private PlayerDecision DetermineCorrectDecision(NPCData npc, List<string> discrepancies)
    {
        if (npc.isSkinwalker)
        {
            return PlayerDecision.Kill;
        }

        if (discrepancies.Count > 0)
        {
            return PlayerDecision.Reject;
        }

        return PlayerDecision.Accept;
    }

    private void CalculateOutcome(NPCData npc, DecisionResults result)
    {
        if (result.wasCorrect)
        {
            result.outcome = DecisionOutcome.Correct;

            if (result.playerDecision == PlayerDecision.Kill)
            {
                result.reasons.Add("Correctly identified and killed a skinwalker");
            }

            else
            {
                result.reasons.Add("correct decision");
            }

            return;
        }

        if (result.playerDecision == PlayerDecision.Accept && npc.isSkinwalker)
        {
            result.outcome = DecisionOutcome.SkinwalkerAccepted;
            result.reasons.Add("skinwalker was allowed through");
            return;
        }

        if (result.playerDecision == PlayerDecision.Kill && result.correctDecision == PlayerDecision.Reject)
        {
            result.outcome = DecisionOutcome.HumanKilled;
            result.reasons.Add("Applicant was not a skinwalker");
            return;
        }

        result.outcome = DecisionOutcome.Incorrect;
        result.reasons.Add("Incorrect decision");

    }
}
