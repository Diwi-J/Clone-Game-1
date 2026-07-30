using System.Collections.Generic;
using Core;
using Core.Events;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO onDayStarted;
    [SerializeField] private VoidEventChannelSO onDayEndStarted;
    private Nationality homeCountry => Nationality.Arstotzka;
    public Nationality HomeCountry => homeCountry;

    public int CurrentDay => GameManager.Instance.Data.CurrentDay;



    // saves the directives here
    public List<DirectiveType> CurrentDirectives { get; private set; }
    = new List<DirectiveType>();

    private void OnEnable()
    {
        if (onDayEndStarted != null) onDayEndStarted.OnEventRaised += HandleDayEndStarted;
        if (onDayStarted != null) onDayStarted.OnEventRaised += HandleDayStarted;
    }

    private void OnDisable()
    {
        if (onDayEndStarted != null) onDayEndStarted.OnEventRaised -= HandleDayEndStarted;
        if (onDayStarted != null) onDayStarted.OnEventRaised -= HandleDayStarted;
    }

    /*private void HandleDayStarted()
    {
        Debug.Log($"Day {CurrentDay} started");
        CurrentDirectives = Directives.GetDirectives(CurrentDay);

    }
    */

    private void HandleDayStarted()
    {
        int currentDay =
            GameManager.Instance.Data.CurrentDay;

        List<DirectiveType> directives =
            Directives.GetDirectives(currentDay);

        GameManager.Instance.Data.CurrentDirectives =
            directives;

        Debug.Log($"Loaded {directives.Count} directives for Day {currentDay}.");
    }

    private void HandleDayEndStarted()
    {
        Debug.Log($" DayManager: Day {CurrentDay} ended");
    }

    public bool HasDirective(DirectiveType directive)
    {
        return CurrentDirectives.Contains(directive);
    }

}
