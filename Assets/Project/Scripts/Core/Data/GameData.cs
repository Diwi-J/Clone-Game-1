using System;
using UnityEngine;
using System.Collections.Generic;

namespace Core.Data
{
    /// <summary>
    /// Central, RUNTIME-ONLY game state. One instance of this lives inside GameManager and
    /// is created fresh every time the game starts (see GameManager.Data field). It is NOT
    /// saved to disk and does NOT persist between play sessions - if you need save/load
    /// later, that's a separate system built on top of this one, not a change to this class.
    ///
    /// WHY A PLAIN CLASS AND NOT A SCRIPTABLEOBJECT?
    /// ScriptableObject assets live in the Project folder, and their field values PERSIST
    /// between play sessions in the Editor (if a script sets CurrentMoney = 500 while you're
    /// testing, it can *still be 500* the next time you hit Play, unless something explicitly
    /// resets it).
    ///
    /// HOW EVERYONE SHOULD USE THIS:
    /// Don't create your own copy of GameData. Always read/write through the one instance
    /// GameManager owns:
    ///
    ///     GameManager.Instance.Data.CurrentMoney += 50;
    ///
    /// If the value you're changing is something OTHER systems care about (money, day
    /// number, etc.), go through the matching GameManager helper method instead of writing
    /// directly, so the matching event fires and everyone finds out:
    ///
    ///     GameManager.Instance.AddMoney(50);   // NOT Data.CurrentMoney += 50 directly
    ///
    /// Don't make your system poll GameData every frame to notice changes - listen for the
    /// relevant event channel instead.
    /// </summary>
 
    [Serializable]
    public class GameData
    {
        [Header("Progression")]
        public int CurrentDay = 1;
        public DayPhase CurrentPhase = DayPhase.MainMenu;
        public float QueueDurationSeconds = 300f;

        [Header("Economy")]
        public int CurrentMoneyAmount = 0;
        public int RentCost = 0;
        public int HeatCost = 0;
        public int FoodCost = 0;
        public int MedicineCost = 0;
        public bool MissedRentToday = false;

        [Header("Border Performance (resets every day, see ResetDailyCounters)")]
        public int ApplicantsProcessedToday = 0;
        public int CorrectDecisionsToday = 0;
        public int WrongDecisionsToday = 0;
        public int SkinwalkersTerminatedToday = 0;
        public int SkinwalkersMissedToday = 0;
        public int InnocentsTerminatedToday = 0;

        [Header("Family / Stakes")]
        [Tooltip("Drops when rent/heat/food/medicine can't be covered. Reaching 0 should trigger Game Over.")]
        public int FamilyHealth = 100;

        [Header("Session")]
        public bool IsGameOver = false;
        public string GameOverReason = "";

        [Header("Daily Directives")]
        public List<DirectiveType> CurrentDirectives = new List<DirectiveType>();

        /// <summary>
        /// Call at the start of every new day (DayStartState does this automatically) to
        /// zero out the "today" counters without touching money, family health, or the
        /// running day count.
        /// </summary>
        public void ResetDailyCounters()
        {
            ApplicantsProcessedToday = 0;
            CorrectDecisionsToday = 0;
            WrongDecisionsToday = 0;
            SkinwalkersTerminatedToday = 0;
            SkinwalkersMissedToday = 0;
            InnocentsTerminatedToday = 0;
        }
    }

    /// <summary>
    /// High-level phase of the current play session. This mirrors, but is NOT the same
    /// object as, the State Machine's active IState - it's a plain piece of DATA describing
    /// "where are we" so UI or other systems can check it without needing a reference to the
    /// state machine itself (e.g. `if (Data.CurrentPhase == DayPhase.Queue)`).
    /// GameManager keeps this in sync every time StateMachine changes state.
    /// </summary>
    public enum DayPhase
    {
        MainMenu,
        DayStart,
        Queue,
        DayEnd,
        GameOver
    }
}
