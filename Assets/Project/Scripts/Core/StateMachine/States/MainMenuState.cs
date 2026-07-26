using UnityEngine;
using Core.Data;

namespace Core.StateMachine.States
{
    /// <summary>
    /// The game is sitting at the main menu. No gameplay systems (queue, verification,
    /// economy) should be doing anything while we're here.
    /// </summary>
    public class MainMenuState : IState
    {
        private readonly GameManager gm;

        public MainMenuState(GameManager gameManager)
        {
            gm = gameManager;
        }

        public void Enter()
        {
            gm.Data.CurrentPhase = DayPhase.MainMenu;
            Debug.Log("[MainMenuState] Entered.");
            // UI System owner: show the main menu screen here. Consider a dedicated
            // "OnMainMenuShown" VoidEventChannelSO if UI needs to react to this specifically.
        }

        public void Tick() { }

        public void Exit()
        {
            Debug.Log("[MainMenuState] Exited.");
        }
    }
}
