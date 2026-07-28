using UnityEngine;

[CreateAssetMenu(
    fileName = "ID_",
    menuName = "Clone Game/Documents/Identification/ID"
)]

public class IDDocumentData : IdentificationDocumentData
{
    [Header("ID Information")]
    public string idNumber;
    public string country;
    public string districtIssued;
    public string firstName;
    public string lastName;
    public Sprite portrait;
    public GameDate dateOfBirth;

    // min value to prevent neg heights & weights
    [Min(0)]
    public int heightCm;

    [Min(0)]
    public int weightKg;

    //overrides abstract holdername into first & last name catergories
    public override string HolderName => $"{firstName} {lastName}";

    // sets document type ad Identification so dont have to manually set it 
    private void OnEnable()
    {
        documentType = IdentificationDocumentType.ID;
    }
}
