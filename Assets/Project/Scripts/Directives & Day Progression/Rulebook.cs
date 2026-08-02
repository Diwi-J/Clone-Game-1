using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Rulebook copy for a single day's directive line - e.g.
/// "Only citizens of Meowland are allowed." All of a given day's
/// directive lines are shown together on the rulebook's first page.
/// </summary>
[Serializable]
public class DirectiveEntry
{
    [Tooltip("In-game day this directive line applies to (1 = Day 1).")]
    public int day;

    [Tooltip("Which DirectiveType this line represents - matches Directives.cs so rulebook text and activation logic can't drift apart.")]
    public List<DirectiveType> directiveTypes = new List<DirectiveType>();

    [TextArea(2, 4)]
    [Tooltip("The directive line exactly as it should read on page 1, e.g. 'Only citizens of Meowland are allowed.'")]
    public string bodyText;
}

/// <summary>
/// One country's rulebook page - the passport photo/reference and the
/// cities within that country authorized to issue it.
/// </summary>
[Serializable]
public class PassportEntry
{
    [Tooltip("Issuing nation - matches PassportDocumentData.country.")]
    public Nationality nationality;

    [Tooltip("Passport photo shown on this country's page. Falls back to sampleDocument's portrait if left empty.")]
    public Sprite referenceImage;

    [Tooltip("Cities within this country that are valid issuing cities for this passport.")]
    public List<IssuingCity> issuingCities = new List<IssuingCity>();

    [Tooltip("Page number this country appears on (page 1 is reserved for the day's directives).")]
    public int page;
}

/// <summary>
/// Master rulebook ScriptableObject. Page 1 is always the current day's
/// directives; each following page is one country, showing its passport
/// photo and valid issuing cities.
///
/// This asset holds DISPLAY data only. Directive ACTIVATION per day still
/// lives in Directives.cs / DayManager - use GetDirectiveLinesForDay() or
/// GetEntriesForActiveDirectives() to pull only what's relevant right now.
/// </summary>
[CreateAssetMenu(fileName = "NewRulebook", menuName = "Papers Recreation/Rulebook", order = 1)]
public class Rulebook : ScriptableObject
{
    [Header("Page 1 - Directives")]
    [Tooltip("Every directive line, across all days. Filter with GetDirectiveLinesForDay() or GetEntriesForActiveDirectives().")]
    public List<DirectiveEntry> directives = new List<DirectiveEntry>();

    [Header("Following Pages - Countries")]
    [Tooltip("One entry per country, ordered by page.")]
    public List<PassportEntry> passports = new List<PassportEntry>();

    // ---------- Directive helpers ----------

    /// <summary>All directive lines for a specific day, in list order.</summary>
    public List<DirectiveEntry> GetDirectiveLinesForDay(int day)
    {
        return directives.Where(d => d.day == day).ToList();
    }

    /// <summary>
    /// Takes the list of currently active DirectiveTypes (e.g. from
    /// DayManager.CurrentDirectives) and returns their matching rulebook
    /// lines. Anything without rulebook text yet is silently skipped
    /// rather than throwing.
    /// </summary>
    public List<DirectiveEntry> GetEntriesForActiveDirectives(List<DirectiveType> activeDirectives)
    {
        return directives.Where(d => d.directiveTypes.Any(activeDirectives.Contains)).ToList();
    }

    // ---------- Passport/country helpers ----------

    /// <summary>All country pages ordered by page number.</summary>
    public List<PassportEntry> GetPassportsOrderedByPage()
    {
        return passports.OrderBy(p => p.page).ToList();
    }

    /// <summary>Look up a country's rulebook page by Nationality.</summary>
    public PassportEntry GetPassportByNationality(Nationality nationality)
    {
        return passports.FirstOrDefault(p => p.nationality == nationality);
    }

    /// <summary>Whether the given city is a valid issuing city for the given nationality, per this rulebook.</summary>
    public bool IsValidIssuingCity(Nationality nationality, IssuingCity city)
    {
        var entry = GetPassportByNationality(nationality);
        return entry != null && entry.issuingCities.Contains(city);
    }

#if UNITY_EDITOR
    // ---------- Editor convenience ----------

    [ContextMenu("Sort Countries By Page")]
    private void SortPassportsByPage()
    {
        passports = passports.OrderBy(p => p.page).ToList();
    }

    [ContextMenu("Validate No Duplicate Country Pages")]
    private void ValidateNoDuplicatePages()
    {
        var duplicates = passports.GroupBy(p => p.page)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var dup in duplicates)
            Debug.LogWarning($"[Rulebook] Duplicate country page number detected: {dup}", this);
    }

    [ContextMenu("Validate No Duplicate Nationality Entries")]
    private void ValidateNoDuplicateNationalities()
    {
        var duplicates = passports.GroupBy(p => p.nationality)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var dup in duplicates)
            Debug.LogWarning($"[Rulebook] Nationality.{dup} has more than one country page.", this);
    }
#endif
}
