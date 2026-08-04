using UnityEngine;
using Core.Data;

namespace Core.StateMachine.States
{
    /// <summary>
    /// The main gameplay loop. Applicants come one at a time; the Documentation, Rule,
    /// Verification, and Dialogue systems all do their work here, driven by the Customer
    /// Queue system.
    ///
    /// IMPORTANT: this state does NOT manage individual applicants directly - that's the
    /// Customer Queue system's job. This state's only responsibilities are:
    ///   1. Tell the Queue system it's OK to start pulling applicants (Enter).
    ///   2. Sit here doing nothing per-frame, because the queue/verification/dialogue
    ///      systems all run off their OWN events and Update loops (Tick).
    ///
    /// GameManager (not this class) is the one listening for gm.OnQueueEmpty and deciding
    /// to transition to DayEndState - see GameManager.HandleQueueEmpty().
    /// </summary>
    public class QueueStartedState : IState
    {
        private readonly GameManager gm;
        private readonly float duration;
        private float timeRemaining;

        public QueueStartedState(GameManager gameManager, float duration)
        {
            gm = gameManager;
            this.duration = duration;
        }

        public void Enter()
        {
            gm.Data.CurrentPhase = DayPhase.Queue;
            Debug.Log("[QueueState] Entered.");

            // Queue System owner: start spawning/pulling applicants when you see this event.
            gm.OnQueueStarted.Raise();
        }

        public void Tick()
        {
            //timeRemaining -= Time.deltaTime;
            //if (timeRemaining <= 0f)
            //{
            //    gm.EndQueue();
            //}
        }

        public void Exit()
        {
            Debug.Log("[QueueState] Exited - day's queue is finished.");
        }
    }
}
