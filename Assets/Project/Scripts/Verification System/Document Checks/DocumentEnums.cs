using UnityEngine;

//sub documents included in the identificaiton document 
public enum IdentificationDocumentType
{
    ID, 
    Passport, 
    WorkPermit 
}

//sub documents included in supporting document
public enum SupportingDocumentType
{
    ClearanceCertificate, 
    VaccineCertificate
}

//NPC gender
public enum Sex
{
    Male, 
    Female
}

//linked to entry permit document 
public enum EntryPurpose
{
    Tourism, 
    Work, 
    Transit, 
    Immigration, 
    Medical, 
    VisitingFamily
}

// linked to criminal record / clearance certificate
public enum OffenceCatergory
{
    None, 
    Minor, 
    Major,
    Wanted
}

// linked to vaccine document 
public enum VaccineType
{
    SWV1, //SkinWalkerVirus1 etc
    SWV2,
    SWV3
}

// linked to vaccine document 
public enum ExposureStatus
{
    None, 
    Suspected, 
    Confirmed
}

public enum Nationality
{
    Arstotzka,
    Antegria,
    Impor,
    Kolechia,
    Obristan,
    Republia,
    UnitedFederation
}

public enum IssuingCity
{
    // Arstotzka
    EastGrestin,
    Paradizna,
    OrvechVonor,

    // Antegria
    Glorian,
    StMarmero,
    OuterGrouse,

    // Impor
    Enkyo,
    Haihan,
    Tsunkeido,

    // Kolechia
    YurkoCity,
    Vedor,
    WestGrestin,

    // Obristan
    Skal,
    Lorndaz,
    Mergerous,

    // Republia
    TrueGlorian,
    Lesrenadi,
    Bostan,

    // United Federation
    GreatRapid,
    Shingleton,
    KoristaCity
}