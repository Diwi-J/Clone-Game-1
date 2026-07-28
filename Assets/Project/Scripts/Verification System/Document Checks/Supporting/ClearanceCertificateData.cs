using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ClearanceCertificate_",
    menuName = "Clone Game/Documents/Supporting/Clearance Certificate"
)]
public class ClearanceCertificateData : SupportingDocumentData
{
    [Header("Identity")]
    public string firstName;
    public string lastName;
    public GameDate dateOfBirth;
    public string nationality;
    public Sprite Photo;

    [Header("Criminal Record")]
    public List<OffenceData> offences = new List<OffenceData>();

    [Header("Validity")]
    public GameDate issueDate;
    public GameDate validUntil;
    public override string HolderName => $"{firstName} {lastName}";

    // default they will have no offences, if they do it will display the highest category of offence
    public OffenceCatergory GetHighestOffenceCatergory()
    {
        OffenceCatergory highestCategory = OffenceCatergory.None;

        foreach (OffenceData offence in offences)
        {
            if (offence.category > highestCategory)
            {
                highestCategory = offence.category;
            }
        }

        return highestCategory;
    }

    // sets document type as supporting so dont have to manually set it
    private void OnEnable()
    {
        documentType = SupportingDocumentType.ClearanceCertificate;
    }

}
