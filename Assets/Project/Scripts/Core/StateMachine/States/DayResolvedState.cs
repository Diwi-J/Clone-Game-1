using UnityEngine;
using Core;
using Core.Data;


namespace Core.StateMachine.States
{
    public class DayResolvedState : IState
    {
        private readonly GameManager gm;
        public DayResolvedState(GameManager gameManager)
        {
            gm = gameManager;
        }

        public void Enter()
        {
            var data = gm.Data;
            data.CurrentPhase = DayPhase.DayEnd;

            Debug.Log($"[DayResolvedState] Entered. Day {gm.Data.CurrentDay - 1} Ends.");

            if (data.MissedRentToday)
            {
                gm.TriggerGameOver("Could not pay rent.");
                return;
            }
            if (data.FamilyHealth <= 0)
            {
                gm.TriggerGameOver("Family did not survive.");
                return;
            }

            data.ResetDailyCounters();
            gm.StartNewDay();
        }
        public void Tick() { }
        public void Exit() 
        {
            Debug.Log("[DayresolvedState] Exited - handing off to DayStart.");
        }
    }
}

