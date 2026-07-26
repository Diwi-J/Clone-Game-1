using System.Collections.Generic;
using UnityEngine;
using Core.Data;

namespace Core.UI
{
    /// <summary>
    /// Desk UI Controller managing the queue of incoming NPCs on the desk,
    /// triggering document updates on DocumentDisplayUI, enabling/disabling decision controls,
    /// and raising OnQueueEmpty when today's queue is cleared.
    /// </summary>
    public class DeskCanvasManager : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private DocumentDisplayUI documentDisplayUI;
        [SerializeField] private DecisionUIController decisionUIController;

        [Header("NPC Applicant Queue")]
        [Tooltip("List of NPCs for the current day. Can be populated via inspector or procedurally.")]
        [SerializeField] private List<NPCData> todayApplicantQueue = new List<NPCData>();

        private int currentApplicantIndex = 0;
        public NPCData CurrentNPC { get; private set; }

        private void OnEnable()
        {
            if (GameManager.Instance != null && GameManager.Instance.OnQueueStarted != null)
            {
                GameManager.Instance.OnQueueStarted.OnEventRaised += HandleQueueStarted;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.OnQueueStarted != null)
            {
                GameManager.Instance.OnQueueStarted.OnEventRaised -= HandleQueueStarted;
            }
        }

        /// <summary>
        /// Set queue of applicants for the current day.
        /// </summary>
        public void SetDailyQueue(List<NPCData> applicants)
        {
            todayApplicantQueue = applicants ?? new List<NPCData>();
            currentApplicantIndex = 0;
        }

        private void HandleQueueStarted()
        {
            currentApplicantIndex = 0;
            PresentNextApplicant();
        }

        public void PresentNextApplicant()
        {
            if (todayApplicantQueue == null || currentApplicantIndex >= todayApplicantQueue.Count)
            {
                // Queue is finished for today
                CurrentNPC = null;
                if (documentDisplayUI != null) documentDisplayUI.ClearAllDocuments();
                if (decisionUIController != null) decisionUIController.SetButtonsInteractable(false);

                Debug.Log("[DeskCanvasManager] All daily applicants processed. Raising OnQueueEmpty.");
                if (GameManager.Instance != null && GameManager.Instance.OnQueueEmpty != null)
                {
                    GameManager.Instance.OnQueueEmpty.Raise();
                }
                return;
            }

            CurrentNPC = todayApplicantQueue[currentApplicantIndex];
            if (documentDisplayUI != null)
            {
                documentDisplayUI.DisplayNPCDocuments(CurrentNPC);
            }

            if (decisionUIController != null)
            {
                decisionUIController.SetButtonsInteractable(true);
            }
        }

        public void OnDecisionCompleted()
        {
            currentApplicantIndex++;
            PresentNextApplicant();
        }
    }
}
