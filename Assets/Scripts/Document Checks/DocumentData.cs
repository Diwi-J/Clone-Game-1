using UnityEngine;

public abstract class DocumentData : ScriptableObject
{
    [Header("Document Authenticity")]
    public bool isForged;

    [Tooltip("Can be false when document is missing its required seal or stamp.")]
    public bool hasOfficialMark = true;
    
}
