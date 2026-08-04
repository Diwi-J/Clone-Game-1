using UnityEngine;
using Core.Data;

namespace Core.StateMachine.States
{
    /// <summary>
    /// The queue emptied out. Now the Economy system pays salary, deducts rent/heat/food/
    /// medicine, and applies any bribes/punishments accrued during the day.
    ///
    /// The Economy system should listen for gm.OnDayEndStarted, do its calculations, apply
    /// them to gm.Data, and then raise gm.OnDayResolved when it's done. GameManager listens
    /// for THAT and decides whether to start a new day or trigger Game Over.
    /// </summary>
    public class FinancesStartedState : IState
    {
        private readonly GameManager gm;

        public FinancesStartedState(GameManager gameManager)
        {
            gm = gameManager;
        }

        public void Enter()
        {
            gm.Data.CurrentPhase = DayPhase.DayEnd;
            Debug.Log("[DayEndState] Entered. Tallying the day.");

            // Economy System owner: calculate salary/expenses/bribes/punishments when you
            // see this event, apply the results to gm.Data (or via gm.AddMoney etc.), then
            // raise gm.OnDayResolved once everything is settled.
            gm.OnFinancesStarted?.Raise();
        }

        public void Tick() { }

        public void Exit()
        {
            Debug.Log("[FinanceStartedState] Exited.");
        }
    }
}
