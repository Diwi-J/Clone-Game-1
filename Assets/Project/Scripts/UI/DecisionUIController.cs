using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    /// <summary>
    /// UI Controller handling player decision buttons (Accept, Reject, Kill) and interacting
    /// with DecisionManager and GameManager state/data.
    /// </summary>
    public class DecisionUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DecisionManager decisionManager;
        [SerializeField] private DeskCanvasManager deskCanvasManager;

        [Header("UI Action Buttons")]
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button rejectButton;
        [SerializeField] private Button killButton;

        [Header("Feedback Banner UI")]
        [SerializeField] private GameObject feedbackBanner;
        [SerializeField] private Text feedbackText;
        [SerializeField] private float feedbackDisplayDuration = 2f;

        private bool isProcessingDecision = false;

        private void Start()
        {
            // Hide feedback banner by default on start
            if (feedbackBanner != null)
            {
                feedbackBanner.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (acceptButton != null) acceptButton.onClick.AddListener(OnAcceptClicked);
            if (rejectButton != null) rejectButton.onClick.AddListener(OnRejectClicked);
            if (killButton != null) killButton.onClick.AddListener(OnKillClicked);
        }

        private void OnDisable()
        {
            if (acceptButton != null) acceptButton.onClick.RemoveListener(OnAcceptClicked);
            if (rejectButton != null) rejectButton.onClick.RemoveListener(OnRejectClicked);
            if (killButton != null) killButton.onClick.RemoveListener(OnKillClicked);
        }

        public void SetButtonsInteractable(bool interactable)
        {
            if (acceptButton != null) acceptButton.interactable = interactable;
            if (rejectButton != null) rejectButton.interactable = interactable;
            if (killButton != null) killButton.interactable = interactable;
        }

        private void OnAcceptClicked() => ExecuteDecision(PlayerDecision.Accept);
        private void OnRejectClicked() => ExecuteDecision(PlayerDecision.Reject);
        private void OnKillClicked() => ExecuteDecision(PlayerDecision.Kill);

        private void ExecuteDecision(PlayerDecision decision)
        {
            if (isProcessingDecision) return;

            NPCData currentNPC = deskCanvasManager != null ? deskCanvasManager.CurrentNPC : null;
            if (currentNPC == null)
            {
                Debug.LogWarning("[DecisionUIController] No active NPC to evaluate!");
                return;
            }

            if (decisionManager == null)
            {
                Debug.LogError("[DecisionUIController] DecisionManager reference is missing in scene!");
                return;
            }

            isProcessingDecision = true;
            SetButtonsInteractable(false);

            Debug.Log($"[DecisionUIController] Processing decision '{decision}' for NPC: {currentNPC.FullName}");

            DecisionResults result = decisionManager.ProcessDecision(currentNPC, decision);
            ApplyDecisionToGameData(result);

            StartCoroutine(ShowFeedbackAndAdvance(result));
        }

        private void ApplyDecisionToGameData(DecisionResults result)
        {
            if (GameManager.Instance == null || GameManager.Instance.Data == null) return;

            var data = GameManager.Instance.Data;
            data.ApplicantsProcessedToday++;

            if (result.wasCorrect)
            {
                data.CorrectDecisionsToday++;
                if (result.playerDecision == PlayerDecision.Kill)
                {
                    data.SkinwalkersTerminatedToday++;
                }
            }
            else
            {
                data.WrongDecisionsToday++;
                if (result.outcome == DecisionOutcome.HumanKilled)
                {
                    data.InnocentsTerminatedToday++;
                }
                else if (result.outcome == DecisionOutcome.SkinwalkerAccepted)
                {
                    data.SkinwalkersMissedToday++;
                    data.FamilyHealth = Mathf.Max(0, data.FamilyHealth - 25);
                }
            }
        }

        private IEnumerator ShowFeedbackAndAdvance(DecisionResults result)
        {
            if (feedbackBanner != null && feedbackText != null)
            {
                feedbackBanner.SetActive(true);

                if (result.wasCorrect)
                {
                    feedbackText.color = Color.green;
                    feedbackText.text = result.playerDecision == PlayerDecision.Kill ?
                        "THREAT NEUTRALIZED!\n(Skinwalker Terminated)" : "PASSED - CORRECT DECISION";
                }
                else
                {
                    feedbackText.color = Color.red;
                    switch (result.outcome)
                    {
                        case DecisionOutcome.HumanKilled:
                            feedbackText.text = "CIVILIAN KILLED!\nSevere Fine Applied!";
                            break;
                        case DecisionOutcome.SkinwalkerAccepted:
                            feedbackText.text = "SKINWALKER BREACH!\nThreat Entered City!";
                            break;
                        default:
                            feedbackText.text = "INCORRECT DECISION!";
                            break;
                    }
                }

                yield return new WaitForSeconds(feedbackDisplayDuration);
                feedbackBanner.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            isProcessingDecision = false;

            // Notify desk manager to move to next applicant or complete queue
            if (deskCanvasManager != null)
            {
                deskCanvasManager.OnDecisionCompleted();
            }
        }
    }
}
