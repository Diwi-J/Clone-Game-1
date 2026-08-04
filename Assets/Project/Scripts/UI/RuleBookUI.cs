using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a Rulebook asset as a paged UI panel:
///   Page 1: current day's directives
///   Next pages: one per country (passport photo + issuing cities)
///   Next: Valid Seals (grid of seal icons + names)
///   Next: Skinwalker Information (subheadings + bullet lists)
///
/// Designed to be a reusable prefab - it only needs a Rulebook asset
/// reference and reads live directive data from GameManager.Instance.Data.
/// </summary>
public class RulebookUI : MonoBehaviour
{
    private enum PageType { Directives, Country, Seals, Skinwalker }

    private class PageRef
    {
        public PageType Type;
        public int DataIndex; // only used for Country pages
    }

    [Header("Data")]
    [Tooltip("The Rulebook ScriptableObject asset to display.")]
    [SerializeField] private Rulebook rulebookData;

    [Header("Shared UI References")]
    [Tooltip("Used as the page heading for every page type.")]
    [SerializeField] private TMP_Text pageTitleText;
    [Tooltip("Used for directives, country, and skinwalker page body text. Hidden on the seals page.")]
    [SerializeField] private TMP_Text pageBodyText;
    [Tooltip("Passport photo, shown only on country pages.")]
    [SerializeField] private Image passportImage;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;

    [Header("Seals Page")]
    [Tooltip("Parent with a Grid/Horizontal/Vertical Layout Group that seal items get instantiated into. Shown only on the seals page.")]
    [SerializeField] private Transform sealsGridContainer;
    [Tooltip("Prefab containing an Image and a TMP_Text (in its children) - instantiated once per seal.")]
    [SerializeField] private GameObject sealItemPrefab;

    [Header("Optional - Panel Root")]
    [Tooltip("If set, Open()/Close() toggle this instead of the whole GameObject. Leave empty to toggle this GameObject.")]
    [SerializeField] private GameObject panelRoot;

    private readonly List<PageRef> pages = new List<PageRef>();
    private List<PassportEntry> orderedPassports = new List<PassportEntry>();
    private int currentPageIndex = 0;

    private void Awake()
    {
        if (nextPageButton != null) nextPageButton.onClick.AddListener(OnNextPageButtonPressed);
        if (previousPageButton != null) previousPageButton.onClick.AddListener(OnPreviousPageButtonPressed);
    }

    private void OnEnable()
    {
        BuildPageList();
        RenderPage(0);
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
    /// Protects against initialization order - if this panel's OnEnable runs
    /// before GameManager's Awake, a plain null-check-and-skip would miss
    /// the subscription forever. Usually resolves same frame.
    /// </summary>
    private System.Collections.IEnumerator SubscribeToDayStartedWhenReady()
    {
        while (GameManager.Instance == null || GameManager.Instance.OnDayStarted == null)
            yield return null;

        GameManager.Instance.OnDayStarted.OnEventRaised += HandleDayStarted;
    }

    private void HandleDayStarted()
    {
        // Only refresh if we're currently on the directives page (avoids
        // yanking the player off a page they're reading if a day happens
        // to change while the rulebook is open).
        if (pages.Count > 0 && pages[currentPageIndex].Type == PageType.Directives)
            StartCoroutine(RefreshDirectivesNextFrame());
    }

    private System.Collections.IEnumerator RefreshDirectivesNextFrame()
    {
        // Waits one frame so DayManager is guaranteed to have finished writing
        // Data.CurrentDirectives, regardless of event subscriber order.
        yield return null;
        RenderPage(currentPageIndex);
    }

    public void Open()
    {
        (panelRoot != null ? panelRoot : gameObject).SetActive(true);
        // OnEnable handles resetting to page 0 (directives).
    }

    public void Close()
    {
        (panelRoot != null ? panelRoot : gameObject).SetActive(false);
    }

    // ---------- Page list construction ----------

    private void BuildPageList()
    {
        pages.Clear();

        if (rulebookData == null)
        {
            Debug.LogError("[RulebookUI] No Rulebook asset assigned.", this);
            orderedPassports = new List<PassportEntry>();
            return;
        }

        orderedPassports = rulebookData.GetPassportsOrderedByPage();

        pages.Add(new PageRef { Type = PageType.Directives });

        for (int i = 0; i < orderedPassports.Count; i++)
            pages.Add(new PageRef { Type = PageType.Country, DataIndex = i });

        if (rulebookData.validSeals != null && rulebookData.validSeals.Count > 0)
            pages.Add(new PageRef { Type = PageType.Seals });

        if (rulebookData.skinwalkerSections != null && rulebookData.skinwalkerSections.Count > 0)
            pages.Add(new PageRef { Type = PageType.Skinwalker });
    }

    // ---------- Rendering ----------

    private void RenderPage(int index)
    {
        if (pages.Count == 0) return;

        currentPageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
        PageRef page = pages[currentPageIndex];

        // Reset shared elements to a known state before each page renders.
        if (passportImage != null) passportImage.gameObject.SetActive(false);
        if (sealsGridContainer != null) sealsGridContainer.gameObject.SetActive(false);
        if (pageBodyText != null) pageBodyText.gameObject.SetActive(true);

        switch (page.Type)
        {
            case PageType.Directives:
                RenderDirectivesPage();
                break;
            case PageType.Country:
                RenderCountryPage(page.DataIndex);
                break;
            case PageType.Seals:
                RenderSealsPage();
                break;
            case PageType.Skinwalker:
                RenderSkinwalkerPage();
                break;
        }

        UpdateNavButtons();
    }

    private void RenderDirectivesPage()
    {
        if (pageTitleText != null) pageTitleText.text = "Ministry Directives";
        if (rulebookData == null || pageBodyText == null) return;

        int currentDay = GameManager.Instance != null ? GameManager.Instance.Data.CurrentDay : 1;
        List<DirectiveEntry> entries = rulebookData.GetDirectiveLinesForDay(currentDay);

        pageBodyText.text = entries.Count == 0
            ? "No new directives today."
            : string.Join("\n", entries.Select(e => e.bodyText));
    }

    private void RenderCountryPage(int index)
    {
        if (orderedPassports == null || orderedPassports.Count == 0) return;

        index = Mathf.Clamp(index, 0, orderedPassports.Count - 1);
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
    }

    private void RenderSealsPage()
    {
        if (pageTitleText != null) pageTitleText.text = rulebookData.sealsPageHeading;
        if (pageBodyText != null) pageBodyText.gameObject.SetActive(false);

        if (sealsGridContainer == null || sealItemPrefab == null)
        {
            Debug.LogError("[RulebookUI] Seals grid container or seal item prefab not assigned.", this);
            return;
        }

        sealsGridContainer.gameObject.SetActive(true);

        // Clear any previously spawned seal items before repopulating.
        for (int i = sealsGridContainer.childCount - 1; i >= 0; i--)
            Destroy(sealsGridContainer.GetChild(i).gameObject);

        foreach (SealEntry seal in rulebookData.validSeals)
        {
            GameObject item = Instantiate(sealItemPrefab, sealsGridContainer);

            Image icon = item.GetComponentInChildren<Image>();
            if (icon != null) icon.sprite = seal.sealImage;

            TMP_Text label = item.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = seal.sealName;
        }
    }

    private void RenderSkinwalkerPage()
    {
        if (pageTitleText != null) pageTitleText.text = rulebookData.skinwalkerMainHeading;
        if (pageBodyText == null) return;

        var sb = new StringBuilder();
        foreach (SkinwalkerSection section in rulebookData.skinwalkerSections)
        {
            sb.AppendLine($"<b>{section.subHeading}</b>");
            foreach (string point in section.bulletPoints)
                sb.AppendLine($"• {point}");
            sb.AppendLine();
        }

        pageBodyText.text = sb.ToString().TrimEnd();
    }

    // ---------- Navigation ----------

    public void OnNextPageButtonPressed()
    {
        if (currentPageIndex < pages.Count - 1)
            RenderPage(currentPageIndex + 1);
    }

    public void OnPreviousPageButtonPressed()
    {
        if (currentPageIndex > 0)
            RenderPage(currentPageIndex - 1);
    }

    private void UpdateNavButtons()
    {
        if (previousPageButton != null)
            previousPageButton.interactable = currentPageIndex > 0;

        if (nextPageButton != null)
            nextPageButton.interactable = currentPageIndex < pages.Count - 1;
    }
}
