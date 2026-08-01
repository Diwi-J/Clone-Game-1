using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(
    fileName = "VaccineCertificate_",
    menuName = "Clone Game/Documents/Supporting/Vaccine Certificate"
)]
public class VaccineCertificationData : SupportingDocumentData
{
    [Header("Identity")]
    public string firstName;
    public string lastName;
    public GameDate dateOfBirth;

    [Header("Vaccine Information")]
    public VaccineType vaccineType;

    [Min(0)]
    public int doses;

    public ExposureStatus exposureStatus;

    [Header("Validity")]
    public GameDate issueDate;
    public GameDate validUntil;

    [Header("Medical")]
    public string facility;
    public Image seal;

    public override string HolderName => $"{firstName} {lastName}";

    // sets document type as supporting so dont have to manually set it
    private void OnEnable()
    {
        documentType = SupportingDocumentType.VaccineCertificate;
    }
}
