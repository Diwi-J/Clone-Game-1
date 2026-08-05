using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Events;
using TMPro;
using UnityEngine;

// attempt to display current directives on screen 
public class DirectivesUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text directivesText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayStarted.OnEventRaised += HandleDayStarted;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayStarted.OnEventRaised -= HandleDayStarted;
        }
    }


    private void HandleDayStarted()
    {
        StartCoroutine(RefreshNextFrame());
    }

    // waits 1 frame before resetting the UI, just to make sure the directives are loaded
    private IEnumerator RefreshNextFrame()
    {
        // Waits one frame so the rule system can load the directives first.
        yield return null;

        RefreshDirectives();
    }

    //reads current directives from game manager then displays them
    public void RefreshDirectives()
    {
        if (directivesText == null)
        {
            Debug.LogError("DirectivesUI: Directives Text has not been assigned.");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("DirectivesUI: GameManager instance could not be found.");
            return;
        }

        List<DirectiveType> directives = GameManager.Instance.Data.CurrentDirectives;

        if (directives == null || directives.Count == 0)
        {
            directivesText.text = "No new Directives today";
            return;
        }

        directivesText.text = "";

        foreach (DirectiveType directive in directives)
        {
            directivesText.text += $"{GetDirectiveDescription(directive)}";
        }
    }

    // gives descript to be loaded into the UI panel
    private string GetDirectiveDescription(DirectiveType directive)
    {
        switch (directive)
        {
            case DirectiveType.IdentificationRequired:
                return "All entrants must show identification";

            case DirectiveType.CitizensOnly:
                return "Only citizens of Meowland may enter";

            case DirectiveType.ForeignersRequireEntryPermit:
                return "Foreigners are allowed to enter if they have a valid entry permit";
            
            case DirectiveType.SupportingDocumentRequired:
                return "Supporting documentation is required";

            case DirectiveType.RejectMajorOffence:
                return "Reject entrants with major offences";

            case DirectiveType.KillSkinwalker:
                return "Eliminate confrimed skinwalkers";

            case DirectiveType.KillConfirmedExposure:
                return "Elimate entrants with confirmed exposure";

            default:
                return directive.ToString();
        }
    }
}
