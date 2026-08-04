using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(
    fileName = "EntryPermit_",
    menuName = "Clone Game/Documents/Entry Permit"
)]
public class EntryPermitData : DocumentData
{
    [Header("Entry Permit Information")]
    public string firstName;
    public string lastName;
    public string passportNumber;

    public EntryPurpose purpose;

    // minimum 1 so not visiting for negative days 
    [Min(1)]
    public int durationDays = 1;

    public GameDate entryByDate;

    public Image seal;

    public string FullName => $"{firstName} {lastName}";
}
