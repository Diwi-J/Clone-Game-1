using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a Rulebook asset as a paged UI panel: starts on the current
/// day's directives, then Next/Previous flips through one page per
/// country (passport photo + issuing cities).
///
/// Designed to be turned into a prefab and reused across scenes - it only
/// needs a Rulebook asset reference and reads live directive data from
/// GameManager.Instance.Data, so it doesn't need any scene-specific wiring.
/// </summary>
public class RulebookUI : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The Rulebook ScriptableObject asset to display.")]
    [SerializeField] private Rulebook rulebookData;

    [Header("UI References")]
    [SerializeField] private TMP_Text pageTitleText;
    [SerializeField] private TMP_Text pageBodyText;
    [SerializeField] private Image passportImage;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;

    [Header("Optional - Panel Root")]
    [Tooltip("If set, Open()/Close() toggle this instead of the whole GameObject. Leave empty to toggle this GameObject.")]
    [SerializeField] private GameObject panelRoot;

    // -1 = directives page, 0+ = index into orderedPassports
    private int currentPassportIndex = -1;
    private List<PassportEntry> orderedPassports = new List<PassportEntry>();

    private void Awake()
    {
        if (nextPageButton != null) nextPageButton.onClick.AddListener(OnNextPageButtonPressed);
        if (previousPageButton != null) previousPageButton.onClick.AddListener(OnPreviousPageButtonPressed);
    }

    private void OnEnable()
    {
        CachePassportOrder();
        ShowDirectivesPage();
        StartCoroutine(SubscribeToDayStartedWhenReady());
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (GameManager.Instance != null && GameManager.Instance.OnDayStarted != null)
            GameManager.Instance.OnDayStarted.OnEventRaised -= HandleDayStarted;
    }

    /// <summary>
    /// Waits until GameManager.Instance actually exists before subscribing.
    /// This protects against initialization order - if this panel's OnEnable
    /// happens to run before GameManager's Awake, a plain null-check-and-skip
    /// would silently miss the subscription forever. Usually resolves same
    /// frame; the loop is just a safety net.
    /// </summary>
    private System.Collections.IEnumerator SubscribeToDayStartedWhenReady()
    {
        while (GameManager.Instance == null || GameManager.Instance.OnDayStarted == null)
            yield return null;

        GameManager.Instance.OnDayStarted.OnEventRaised += HandleDayStarted;
    }

    private void HandleDayStarted()
    {
        // Only refresh if we're currently showing the directives page (avoids
        // yanking the player off a country page they're reading if a day
        // happens to change while it's open).
        if (currentPassportIndex == -1)
            StartCoroutine(RefreshDirectivesNextFrame());
    }

    private System.Collections.IEnumerator RefreshDirectivesNextFrame()
    {
        // Waits one frame so DayManager is guaranteed to have finished writing
        // Data.CurrentDirectives, regardless of event subscriber order.
        yield return null;
        ShowDirectivesPage();
    }

    /// <summary>Call this to open the rulebook from anywhere (e.g. a "View Rulebook" button elsewhere in the UI).</summary>
    public void Open()
    {
        (panelRoot != null ? panelRoot : gameObject).SetActive(true);
        // OnEnable will handle resetting to the directives page.
    }

    public void Close()
    {
        (panelRoot != null ? panelRoot : gameObject).SetActive(false);
    }

    private void CachePassportOrder()
    {
        if (rulebookData == null)
        {
            Debug.LogError("[RulebookUI] No Rulebook asset assigned.", this);
            orderedPassports = new List<PassportEntry>();
            return;
        }

        orderedPassports = rulebookData.GetPassportsOrderedByPage();
    }

    // ---------- Directives page ----------

    private void ShowDirectivesPage()
    {
        currentPassportIndex = -1;

        if (passportImage != null) passportImage.gameObject.SetActive(false);
        if (pageTitleText != null) pageTitleText.text = "Ministry Directives";

        if (rulebookData == null || pageBodyText == null) return;

        int currentDay = GameManager.Instance != null ? GameManager.Instance.Data.CurrentDay : 1;

        List<DirectiveEntry> entries = rulebookData.GetDirectiveLinesForDay(currentDay);

        pageBodyText.text = entries.Count == 0
            ? "No new directives today."
            : string.Join("\n", entries.Select(e => e.bodyText));

        UpdateNavButtons();
    }

    // ---------- Country/passport pages ----------

    private void ShowPassportPage(int index)
    {
        if (orderedPassports == null || orderedPassports.Count == 0) return;

        index = Mathf.Clamp(index, 0, orderedPassports.Count - 1);
        currentPassportIndex = index;

        PassportEntry entry = orderedPassports[index];

        if (pageTitleText != null) pageTitleText.text = entry.nationality.ToString();

        if (passportImage != null)
        {
            passportImage.gameObject.SetActive(true);
            passportImage.sprite = entry.referenceImage;
        }

        if (pageBodyText != null)
        {
            pageBodyText.text = entry.issuingCities == null || entry.issuingCities.Count == 0
                ? "No issuing cities listed."
                : "Issuing Cities:\n" + string.Join("\n", entry.issuingCities.Select(c => $"• {c}"));
        }

        UpdateNavButtons();
    }

    // ---------- Navigation ----------

    public void OnNextPageButtonPressed()
    {
        if (currentPassportIndex == -1)
        {
            if (orderedPassports.Count > 0) ShowPassportPage(0);
            return;
        }

        if (currentPassportIndex < orderedPassports.Count - 1)
            ShowPassportPage(currentPassportIndex + 1);
    }

    public void OnPreviousPageButtonPressed()
    {
        if (currentPassportIndex == 0)
        {
            ShowDirectivesPage();
            return;
        }

        if (currentPassportIndex > 0)
            ShowPassportPage(currentPassportIndex - 1);
    }

    private void UpdateNavButtons()
    {
        if (previousPageButton != null)
            previousPageButton.interactable = currentPassportIndex != -1;

        if (nextPageButton != null)
            nextPageButton.interactable = orderedPassports.Count > 0
                && currentPassportIndex < orderedPassports.Count - 1;
    }
}
