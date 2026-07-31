using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using System;

namespace Core.UI
{
    [Serializable]
    public class DailyApplicantQueue
    {
        [Min(1)]
        public int dayNumber = 1;

        public List<NPCData> applicants = new List<NPCData>();
    }
    /// <summary>
    /// Desk UI Controller managing the queue of incoming NPCs on the desk,
    /// triggering document updates on DocumentDisplayUI, enabling/disabling decision controls,
    /// and raising OnQueueEmpty when today's queue is cleared.
    /// </summary>
    public class DeskCanvasManager : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]  private DocumentDisplayUI documentDisplayUI; 
        [SerializeField]  private DecisionUIController decisionUIController;
        [SerializeField]  private AnnouncementUI announcementUI;

        [Header("Daily Applicant Queues")]
        [Tooltip("Create one entry for each day and assign that day's NPCs.")]
        [SerializeField]
        private List<DailyApplicantQueue> dailyQueues =
             new List<DailyApplicantQueue>();

        [Header("Testing")]
        [Tooltip("Used when testing without the GameManager.")]
        [Min(1)]
        [SerializeField]
        private int testDay = 1;

        private List<NPCData> currentDayQueue =
            new List<NPCData>();

        private int currentApplicantIndex;

        public NPCData CurrentNPC { get; private set; }

        private void OnEnable()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.OnQueueStarted != null)
            {
                GameManager.Instance.OnQueueStarted.OnEventRaised +=
                    HandleQueueStarted;
            }
        }
        private void OnDisable()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.OnQueueStarted != null)
            {
                GameManager.Instance.OnQueueStarted.OnEventRaised -=
                    HandleQueueStarted;
            }
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                LoadQueueForDay(testDay);
                PresentNextApplicant();
            }
        }


        /// <summary>
        /// Set queue of applicants for the current day.
        /// </summary>


        private void HandleQueueStarted()
        {
            int currentDay = GetCurrentDay();

            LoadQueueForDay(currentDay);
            PresentNextApplicant();
        }
        private int GetCurrentDay()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.Data != null)
            {
                return GameManager.Instance.Data.CurrentDay;
            }

            Debug.LogWarning(
                "[DeskCanvasManager] GameManager was not available. " +
                $"Using test day {testDay}."
            );

            return testDay;
        }
        private void LoadQueueForDay(int dayNumber)
        {
            DailyApplicantQueue selectedQueue =
                dailyQueues.Find(queue =>
                    queue.dayNumber == dayNumber);

            if (selectedQueue == null)
            {
                Debug.LogWarning(
                    $"[DeskCanvasManager] No applicant queue was assigned " +
                    $"for Day {dayNumber}."
                );

                currentDayQueue = new List<NPCData>();
            }
            else
            {
                /*
                 * Make a new list so the original Inspector list
                 * is not changed while playing.
                 */
                currentDayQueue =
                    new List<NPCData>(selectedQueue.applicants);
            }

            currentApplicantIndex = 0;
            CurrentNPC = null;

            Debug.Log(
                $"[DeskCanvasManager] Loaded Day {dayNumber} with " +
                $"{currentDayQueue.Count} applicants."
            );
        }

        public void PresentNextApplicant()
        {
            if (currentDayQueue == null ||
                 currentApplicantIndex >= currentDayQueue.Count)
            {
                FinishCurrentQueue();
                return;
            }

            CurrentNPC =
                currentDayQueue[currentApplicantIndex];

            if (CurrentNPC == null)
            {
                Debug.LogWarning(
                    $"DeskCanvasManager: Applicant at index " + $"{currentApplicantIndex} is missing."
                );

                currentApplicantIndex++;
                PresentNextApplicant();
                return;
            }

            if (announcementUI != null)
            {
                announcementUI.Show("PAPERS, PLEASE.");
            }

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

        public void FinishCurrentQueue()
        {
            CurrentNPC = null;

            if (documentDisplayUI != null)
            {
                documentDisplayUI.ClearAllDocuments();
            }

            if (decisionUIController != null)
            {
                decisionUIController.SetButtonsInteractable(false);
            }

        }

        public void SetDailyQueue(List<NPCData> applicants)
        {
            currentDayQueue =
                applicants != null
                    ? new List<NPCData>(applicants)
                    : new List<NPCData>();

            currentApplicantIndex = 0;
            CurrentNPC = null;
        }
    }
}
