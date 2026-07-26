namespace Core.StateMachine
{
    /// <summary>
    /// Contract for every state the game can be in (MainMenu, DayStart, Queue, DayEnd, GameOver).
    ///
    /// States are not GameObjects.
    /// they're just logic containers that GameStateMachine calls into. This keeps them cheap
    /// to create (`new DayStartState(gameManager)`) and easy to test without the Unity engine
    /// running.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called ONCE, the instant the state machine switches INTO this state.
        /// Use this for setup: raising "we just entered this phase" events, resetting
        /// counters, telling UI to show a screen, etc.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called every frame (from GameManager's Update, via StateMachine.Tick()) WHILE
        /// this state is active. Most of our states will leave this EMPTY - gameplay systems
        /// like the Queue or Dialogue system should react to their OWN events/updates, not be
        /// driven by the state machine polling them every frame. Only put logic here if it
        /// genuinely needs to run every single frame this state is active.
        /// </summary>
        void Tick();

        /// <summary>
        /// Called ONCE, the instant the state machine switches AWAY from this state.
        /// Use this for cleanup: hiding UI, saving results, unsubscribing from anything
        /// you subscribed to in Enter().
        /// </summary>
        void Exit();
    }
}
