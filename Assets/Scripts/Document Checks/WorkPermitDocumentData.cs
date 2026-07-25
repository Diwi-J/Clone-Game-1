using UnityEngine;

[CreateAssetMenu(
    fileName = "WorkPermit_",
    menuName = "Clone Game/Documents/Identification/Work Permit"
)]
public class WorkPermitDocumentData : IdentificationDocumentData
{
    [Header("Work Permit Information")]
    public string holderName;
    public string workField;
    public GameDate validUntil;
    public override string HolderName => holderName;

    // sets document type as Identification so dont have to manually set it
    private void OnEnable()
    {
        documentType = IdentificationDocumentType.WorkPermit;
    }
}
