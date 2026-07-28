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
    public string nationality;
    public Sex sex;

    [Header("True Appearance")]
    public Sprite portrait;

    [Min(0)]
    public int heightCm;

    [Min(0)]
    public float weightCm;

    [Header("True Entry Information")]
    public EntryPurpose entryPurpose;

    [Min(1)]
    public int intendedStayDays = 1;

    public string passportNumber;

    [Header("NPC Classification")]
    public bool isSkinwalker;

    [Header("PresentedDocuments")]
    public IdentificationDocumentData identificationDocument;
    public EntryPermitData entryPermit;
    public SupportingDocumentData supportingDocument;

    public string FullName => $"{firstName} {lastName}";

}
