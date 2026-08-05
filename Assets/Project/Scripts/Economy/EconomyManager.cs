using UnityEngine;
using Core.Events;

namespace Core.Economy
{
    /// <summary>
    /// Resolves the day's finances at day-end: pays rent (hard game-over if unaffordable),
    /// then heat/food/medicine (soft failure — costs FamilyHealth if unaffordable, money
    /// never goes negative). Raises OnDayResolved if the day survives.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { get; private set; }

        [Header("Wiring")]
        public VoidEventChannelSO OnDayEndStarted;

        [Header("Expense Config")]
        public int healthPenaltyPerMissedExpense = 10;

        [Header("Income & Penalty Tuning")]
        [SerializeField] private int correctDecisionSalary = 50;
        [SerializeField] private int skinwalkerKillBonus = 100;
        [SerializeField] private int innocentKillPenalty = -100;
        [SerializeField] private int skinwalkerEscapedPenalty = -100;
        [SerializeField] private int skinwalkerEscapedHealthPenalty = 25;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void OnEnable()
        {
            if (OnDayEndStarted != null) OnDayEndStarted.OnEventRaised += ResolveDay;
        }

        private void OnDisable()
        {
            if (OnDayEndStarted != null) OnDayEndStarted.OnEventRaised -= ResolveDay;
        }

        /// <summary>Call this once per applicant decision. Handles all money/health effects.</summary>
        public void AwardOutcome(DecisionResults result)
        {
            var gm = GameManager.Instance;

            if (result.wasCorrect)
            {
                gm.AddMoney(result.playerDecision == PlayerDecision.Kill
                    ? correctDecisionSalary + skinwalkerKillBonus
                    : correctDecisionSalary);
                return;
            }

            if (result.outcome == DecisionOutcome.HumanKilled)
            {
                gm.AddMoney(innocentKillPenalty);
            }
            else if (result.outcome == DecisionOutcome.SkinwalkerAccepted)
            {
                gm.AddMoney(skinwalkerEscapedPenalty);
                gm.ChangeFamilyHealth(-skinwalkerEscapedHealthPenalty);
            }
        }

        private void ResolveDay()
        {
            var gm = GameManager.Instance;
            var data = gm.Data;

            if (data.CurrentMoneyAmount < data.RentCost)
            {
                data.MissedRentToday = true;
            }
            else
            {
                gm.AddMoney(-data.RentCost);
                data.MissedRentToday = false;
            }   

            TryPaySoftExpense(gm, data.HeatCost, "heat");
            TryPaySoftExpense(gm, data.FoodCost, "food");
            TryPaySoftExpense(gm, data.MedicineCost, "medicine");

            if (data.FamilyHealth <= 0)
            {
                gm.TriggerGameOver("Family did not survive.");
                return;
            }

            gm.OnDayResolved?.Raise();
        }

        private void TryPaySoftExpense(GameManager gm, int cost, string label)
        {
            var data = gm.Data;
            if (data.CurrentMoneyAmount >= cost)
            {
                gm.AddMoney(-cost);
            }
            else
            {
                gm.AddMoney(-data.CurrentMoneyAmount);
                gm.ChangeFamilyHealth(-healthPenaltyPerMissedExpense);
                Debug.Log($"[EconomyManager] Could not afford {label} — FamilyHealth now {data.FamilyHealth}");
            }
        }
    }
}
