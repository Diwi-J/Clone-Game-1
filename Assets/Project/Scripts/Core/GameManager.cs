using UnityEngine;
using Core.StateMachine;
using Core.StateMachine.States;
using Core.Data;
using Core.Events;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// THE central hub of the game. Every other system should be able to answer "what do I
    /// do right now?" by either:
    ///   a) listening to one of the event channels below, or
    ///   b) reading GameManager.Instance.Data
    ///
    /// Systems should NOT reach into each other directly - e.g. the Queue system should not
    /// hold a direct reference to the Economy system, and the Dialogue system shouldn't
    /// directly call into Verification. Go through GameManager + events instead. This is
    /// what lets everyone build and test their own system in isolation, and it's also the
    /// architectural point our design hypothesis leans on: normal and supernatural checks
    /// flow through the exact same events and the exact same GameData, so the game doesn't
    /// "know" a supernatural check is any different from a normal one.
    ///
    /// RULE FOR THE WHOLE TEAM: only the methods in the "Flow control" region below should
    /// ever call StateMachine.ChangeState(). If your system needs a transition to happen,
    /// raise the matching event and let GameManager react to it (see HandleQueueEmpty /
    /// HandleDayResolved below for examples of that pattern).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Runtime Data")]
        [Tooltip("The single source of truth for day/money/progress this session. Read/write via GameManager.Instance.Data")]
        public GameData Data = new GameData();

        [Header("Event Channels - Flow")]
        [Tooltip("Raised when a new day begins. Rule System: generate today's rules. UI: show day briefing.")]
        public VoidEventChannelSO OnDayStarted;

        [Tooltip("Raised when the queue should start pulling applicants (after the day briefing is dismissed).")]
        public VoidEventChannelSO OnQueueStarted;

        [Tooltip("RAISE THIS FROM THE QUEUE SYSTEM when the last applicant has been processed and the queue is empty.")]
        public VoidEventChannelSO OnQueueEmpty;

        [Tooltip("Raised when day-end resolution (salary/expenses/bribes) should begin.")]
        public VoidEventChannelSO OnDayEndStarted;

        [Tooltip("RAISE THIS FROM THE ECONOMY SYSTEM once the day's money has been fully resolved.")]
        public VoidEventChannelSO OnDayResolved;

        [Tooltip("Raised when the run ends (win or lose). Data.GameOverReason will be set.")]
        public VoidEventChannelSO OnGameOver;

        [Header("Event Channels - Data")]
        [Tooltip("Raised whenever CurrentMoney changes, via AddMoney(). Payload = the new total.")]
        public IntEventChannelSO OnMoneyChanged;

        [Tooltip("Raised whenever FamilyHealth changes, via OnFamilyHealthChanged. PayLoad = new value.")]
        public IntEventChannelSO OnFamilyHealthChanged;

        public GameStateMachine StateMachine { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[GameManager] Duplicate GameManager found in scene - destroying the new one.");
                Destroy(gameObject);
                return;
            }
            Instance = this;

            StateMachine = new GameStateMachine();
        }

        private void OnEnable()
        {
            if (OnQueueEmpty != null) OnQueueEmpty.OnEventRaised += HandleQueueEmpty;
            if (OnDayResolved != null) OnDayResolved.OnEventRaised += HandleDayResolved;
        }

        private void OnDisable()
        {
            if (OnQueueEmpty != null) OnQueueEmpty.OnEventRaised -= HandleQueueEmpty;
            if (OnDayResolved != null) OnDayResolved.OnEventRaised -= HandleDayResolved;
        }

        private void Start()
        {
            // Boots straight into the main menu.
            StateMachine.ChangeState(new MainMenuState(this));
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        #region Flow Control ================================================================================================
        // Only these methods (plus the two Handle* callbacks below) should ever call
        // StateMachine.ChangeState(). Everyone else: raise an event instead.

        /// <summary>Call this from a "Start Game" / "Continue" UI button.</summary>
        public void StartNewDay()
        {
            StateMachine.ChangeState(new DayStartState(this));
        }

        /// <summary>Call this once the Day Start briefing/rules screen has been dismissed.</summary>
        public void BeginQueue()
        {
            StateMachine.ChangeState(new QueueState(this));
        }

        private void HandleQueueEmpty()
        {
            StateMachine.ChangeState(new DayEndState(this));
        }

        private void HandleDayResolved()
        {
            if (Data.IsGameOver)
                StateMachine.ChangeState(new GameOverState(this));
            else
                Data.CurrentDay++;
                StartNewDay();
        }

        /// <summary>Call this from anywhere (Economy system, an event trigger, etc.) to end the run.</summary>
        public void TriggerGameOver(string reason)
        {
            Data.GameOverReason = reason;
            Data.IsGameOver = true;
            StateMachine.ChangeState(new GameOverState(this));
        }

        // ==== Convenience helpers for other systems ==============================

        /// <summary>
        /// Central place to change money so OnMoneyChanged always fires. Use this instead
        /// of writing directly to Data.CurrentMoney from other scripts (a negative amount
        /// subtracts).
        /// </summary>
        
        public void AddMoney(int amount)
        {
            Data.CurrentMoneyAmount += amount;
            OnMoneyChanged?.Raise(Data.CurrentMoneyAmount);
        }

        public void ChangeFamilyHealth(int amount)
        {
            Data.FamilyHealth = Mathf.Clamp(Data.FamilyHealth +  amount, 0, 100);
            OnFamilyHealthChanged?.Raise(Data.FamilyHealth);
        }
        #endregion 
    }
}
