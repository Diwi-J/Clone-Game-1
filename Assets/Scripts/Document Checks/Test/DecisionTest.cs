using UnityEngine;
using UnityEngine.InputSystem;

// Used ChatGPT to debug input errors and update the keyboard input to Unity's new input system
public class DecisionTest : MonoBehaviour
{
    [Header("NPC To Test")]
    [SerializeField] private NPCData npc;

    [Header("Decision Manager")]
    [SerializeField] private DecisionManager decisionManager;

    // Prevents the player from making multiple decision on the same NPC 
    private bool decisionMade = false;

    private void Update()
    {
        // Stop checking for input once a decision has been made.
        if (decisionMade)
        {
            return;
        }


        // A = Accept
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            TestDecision(PlayerDecision.Accept);
        }

        // R = Reject
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TestDecision(PlayerDecision.Reject);
        }

        // K = Kill
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TestDecision(PlayerDecision.Kill);
        }
    }

    private void TestDecision(PlayerDecision playerDecision)
    {
        if (npc == null)
        {
            Debug.LogError("No NPC assigned.");
            return;
        }

        if (decisionManager == null)
        {
            Debug.LogError("No Decision Manager assigned.");
            return;
        }

        decisionMade = true;

        DecisionResults result =
            decisionManager.ProcessDecision(npc, playerDecision);

        // Displays result in console
        Debug.Log("========== DECISION RESULT ==========");
        Debug.Log($"NPC: {npc.FullName}");
        Debug.Log($"Player Decision: {result.playerDecision}");
        Debug.Log($"Correct Decision: {result.correctDecision}");
        Debug.Log($"Correct? {result.wasCorrect}");
        Debug.Log($"Outcome: {result.outcome}");
        Debug.Log("=====================================");
    }
}
