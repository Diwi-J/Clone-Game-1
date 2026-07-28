using System.Collections.Generic;
using UnityEngine;

public class VerificationTester : MonoBehaviour
{
    [Header("Test Data")]
    [SerializeField] private NPCData npc;
    [SerializeField] private DocumentVerification verifier;

    private void Start()
    {
        if (npc == null)
        {
            Debug.LogError("No NPCData asset assigned.");
            return;
        }

        if (verifier == null)
        {
            Debug.LogError("No DocumentVerifier component assigned.");
            return;
        }

        List<string> discrepancies = verifier.VerifyNPC(npc);

        if (discrepancies.Count == 0)
        {
            Debug.Log($"{npc.FullName}: No discrepancies found.");
            return;
        }

        Debug.LogWarning(
            $"{npc.FullName}: {discrepancies.Count} discrepancy/discrepancies found."
        );

        foreach (string discrepancy in discrepancies)
        {
            Debug.LogWarning(discrepancy);
        }
    }
}
