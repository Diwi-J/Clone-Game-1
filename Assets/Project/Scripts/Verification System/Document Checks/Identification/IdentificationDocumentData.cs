using System.Threading;
using UnityEngine;

// parent class for all ID docs
// cant create instance directl but other classes (ID, Passport & Work permit) inherit
public abstract class IdentificationDocumentData : DocumentData
{
    [Header("Identification Type")]

    //stores what kind of document this is 
    public IdentificationDocumentType documentType;

    // every Id doc must have the holders name 
    // each doc stores the name differely, i forced every child class to provide its own implementation
    // ID - first & last name 
    // Passport - first & last name 
    // Work permit - holder name 
    // check GDD for better explanation
    public abstract string HolderName { get; }
}
