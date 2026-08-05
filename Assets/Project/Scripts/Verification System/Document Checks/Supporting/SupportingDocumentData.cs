using UnityEngine;

public abstract class SupportingDocumentData : DocumentData
{
    [Header("Supprting Document Type")]
    public SupportingDocumentType documentType;

    public abstract string HolderName { get; }
}
