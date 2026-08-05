using UnityEngine;

[CreateAssetMenu(
    fileName = "Passport_",
    menuName = "Clone Game/Documents/Identification/Passport"
)]

public class PassportDocumentData : IdentificationDocumentData
{
    [Header("Passport Information")]
    public string passportNumber;
    public string firstName;
    public string lastName;
    public Nationality country;
    public GameDate dateOfBirth;
    public Sex sex;
    public IssuingCity issuingCity;
    public GameDate expirationDate;
    public Sprite portrait;
    public override string HolderName => $"{firstName} {lastName}";

    // sets document type ad Identification so dont have to manually set it
    private void OnEnable()
    {
        documentType = IdentificationDocumentType.Passport;
    }
}
