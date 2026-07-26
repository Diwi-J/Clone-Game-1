using UnityEngine;
using Core.Data;

namespace Core.StateMachine.States
{
    /// <summary>
    /// The brief moment between days: show today's rules/briefing, let the player see what
    /// they owe (rent/heat/food/medicine) before the queue opens.
    ///
    /// This is where the RULE SYSTEM owner should hook in to generate/select today's document
    /// rules, listening for gm.OnDayStarted.
    ///
    /// NOTE: this state does NOT automatically move on to the queue. Something (a "Begin Day"
    /// UI button, most likely) needs to call GameManager.Instance.BeginQueue() once the
    /// briefing has been shown and dismissed. This is a deliberate choice so the briefing
    /// screen can stay up for as long as the UI system needs.
    /// </summary>
    public class DayStartState : IState
    {
        private readonly GameManager gm;

        public DayStartState(GameManager gameManager)
        {
            gm = gameManager;
        }

        public void Enter()
        {
            gm.Data.CurrentPhase = DayPhase.DayStart;
            gm.Data.ResetDailyCounters();

            Debug.Log($"[DayStartState] Entered. Day {gm.Data.CurrentDay} begins.");

            // Rule System: listen for this to generate/select today's document rules.
            // UI System: listen for this to show the "Day X" briefing screen.
            gm.OnDayStarted.Raise();
        }

        public void Tick() { }

        public void Exit()
        {
            Debug.Log("[DayStartState] Exited - handing off to the queue.");
        }
    }
}
