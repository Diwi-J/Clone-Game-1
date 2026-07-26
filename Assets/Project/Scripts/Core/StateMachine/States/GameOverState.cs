using UnityEngine;
using Core.Data;

namespace Core.StateMachine.States
{
    /// <summary>
    /// Terminal state - the run has ended (ran out of money, family died, etc).
    /// No gameplay systems should be doing anything once we're here.
    /// </summary>
    public class GameOverState : IState
    {
        private readonly GameManager gm;

        public GameOverState(GameManager gameManager)
        {
            gm = gameManager;
        }

        public void Enter()
        {
            gm.Data.CurrentPhase = DayPhase.GameOver;
            gm.Data.IsGameOver = true;

            Debug.Log($"[GameOverState] Entered. Reason: {gm.Data.GameOverReason}");

            // UI System owner: listen for this to show the Game Over screen with the reason.
            gm.OnGameOver.Raise();
        }

        public void Tick() { }

        public void Exit() { }
    }
}
