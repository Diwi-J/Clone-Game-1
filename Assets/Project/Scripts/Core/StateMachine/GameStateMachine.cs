using System;
using UnityEngine;

namespace Core.StateMachine
{
    /// <summary>
    /// Owns "which state are we in" and handles switching between states safely.
    ///
    /// This is a PLAIN C# class, NOT a MonoBehaviour. GameManager creates ONE instance of
    /// this in its Awake(), and calls Tick() on it from its own Update(). Keeping it plain
    /// C# means it doesn't depend on the Unity GameObject lifecycle and is easy to reason
    /// about in isolation.
    ///
    /// USAGE:
    ///     stateMachine.ChangeState(new DayStartState(gameManager));
    ///
    /// IMPORTANT RULE FOR THE WHOLE TEAM:
    /// Only GameManager should ever call ChangeState(). If YOUR system needs a transition
    /// to happen (e.g. the Queue system finishes and wants to move to Day End), do NOT call
    /// ChangeState yourself. Instead, RAISE THE MATCHING EVENT (e.g. OnQueueEmpty) and let
    /// GameManager decide what to do with it. This keeps all transition logic in ONE place
    /// instead of scattered across every system, which is what makes the flow easy to debug
    /// when something goes wrong.
    /// </summary>
    public class GameStateMachine
    {
        public IState CurrentState { get; private set; }

        /// <summary>
        /// Fired right after a transition completes. This is for debugging/observability
        /// (e.g. a debug overlay showing the current state's type name) - it is NOT how
        /// gameplay systems should react to phase changes. Use the ScriptableObject event
        /// channels on GameManager for that instead.
        /// </summary>

        public event Action<IState> OnStateChanged;

        public void ChangeState(IState newState)
        {
            if (newState == null)
            {
                Debug.LogError("[GameStateMachine] Tried to change to a null state. Ignoring.");
                return;
            }

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();

            OnStateChanged?.Invoke(CurrentState);
        }

        /// <summary>Call once per frame from whoever owns this state machine (GameManager).</summary>
        public void Tick()
        {
            CurrentState?.Tick();
        }
    }
}
