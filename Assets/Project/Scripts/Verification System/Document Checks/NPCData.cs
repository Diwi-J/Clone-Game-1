using UnityEngine;

[CreateAssetMenu(
    fileName = "NPC_",
    menuName = "Clone Game/NPC Data"
)]
public class NPCData : ScriptableObject
{
    [Header("True identity")]
    public string firstName;
    public string lastName;
    public GameDate dateOfBirth;
    public Nationality nationality;
    public Sex sex;

    [Header("True Appearance")]
    public Sprite portrait;
    public Color characterTint = Color.white;

    [Header("True Entry Information")]
    public EntryPurpose entryPurpose;

    [Min(1)]
    public int intendedStayDays = 1;

    public string passportNumber;

    [Header("NPC Classification")]
    public bool isSkinwalker;

    [Header("PresentedDocuments")]
    public PassportDocumentData passportDocument;
    public IDDocumentData identificationDocument;
    public WorkPermitDocumentData workPermit;
    public EntryPermitData entryPermit;
    public ClearanceCertificateData clearanceDocument;
    public VaccineCertificationData vaccineCertification;

    public string FullName => $"{firstName} {lastName}";

}
