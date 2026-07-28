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
    public string country;
    public GameDate dateOfBirth;
    public Sex sex;
    public string issuingCity;
    public GameDate expirationDate;
    public override string HolderName => $"{firstName} {lastName}";

    // sets document type ad Identification so dont have to manually set it
    private void OnEnable()
    {
        documentType = IdentificationDocumentType.Passport;
    }
}
