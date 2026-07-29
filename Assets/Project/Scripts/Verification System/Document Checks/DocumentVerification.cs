using System.Collections.Generic;
using UnityEngine;

public class DocumentVerification : MonoBehaviour
{
    [SerializeField] private GameDate currentDate;
    [SerializeField] private DayManager dayManager;

    public List<string> VerifyNPC(NPCData npc)
    {
        List<string> discrepancies = new List<string>();

        if (npc == null)
        {
            discrepancies.Add("No NPC data was procided");
            return discrepancies;
        }

        VerifyEntryEligibility(npc, discrepancies);
        VerifyIdentificationDocument(npc, discrepancies);
        VerifyEntryPermit(npc, discrepancies);
        VerifySupportingDocument(npc, discrepancies);

        return discrepancies;
    }

    private void VerifyIdentificationDocument(NPCData npc, List<string> discrepancies)
    {
        if (npc.identificationDocument == null)
        {
            if (dayManager.HasDirective(DirectiveType.IdentificationRequired))
            {
                discrepancies.Add("Identification document is missing.");
            }

            return;
        }


        if (npc.identificationDocument.HolderName != npc.FullName)
        {
            discrepancies.Add("Identification document name does not match");
        }

        if (npc.identificationDocument.isForged)
        {
            discrepancies.Add("identification document is forged");
        }

        switch (npc.identificationDocument)
        {
            case IDDocumentData id:
                VerifyID(npc, id, discrepancies);
                break;

            case PassportDocumentData passport:
                VerifyPassport(npc, passport, discrepancies);
                break;

            case WorkPermitDocumentData permit:
                VerifyWorkPermit(npc, permit, discrepancies);
                break;
        }
    }

    private void VerifyID(NPCData npc, IDDocumentData id, List<string> discrepancies)
    {
        if (id.dateOfBirth.ToDateTime() != npc.dateOfBirth.ToDateTime())
        {
            discrepancies.Add("ID date of birth does not match");
        }

        if (id.country != npc.nationality)
        {
            discrepancies.Add("ID country does not match");
        }

        if (id.heightCm != npc.heightCm)
        {
            discrepancies.Add("ID height does not match");
        }

        if (id.weightKg != npc.weightCm)
        {
            discrepancies.Add("ID weight does not match");
        }
    }

    private void VerifyPassport(NPCData npc, PassportDocumentData passport, List<string> discrepancies)
    {
        if (passport.dateOfBirth.ToDateTime() != npc.dateOfBirth.ToDateTime())
        {
            discrepancies.Add("Passport date of birth does not match");
        }

        if (passport.country != npc.nationality)
        {
            discrepancies.Add("Passport country does not match.");
        }

        if (passport.sex != npc.sex)
        {
            discrepancies.Add("Passport sex does not match");
        }

        if (passport.passportNumber != npc.passportNumber)
        {
            discrepancies.Add("Passport number does not match.");
        }

        if (passport.expirationDate.IsExpired(currentDate))
        {
            discrepancies.Add("Passport is expired");
        }
    }

    private void VerifyWorkPermit(NPCData npc, WorkPermitDocumentData workPermit, List<string> discrepancies)
    {
        if (!workPermit.hasOfficialMark)
        {
            discrepancies.Add("Work permit seal is wrong / missing");
        }

        if (workPermit.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add("Work permit is expired");
        }
    }

    private void VerifyEntryPermit(NPCData npc, List<string> discrepancies)
    {
        EntryPermitData permit = npc.entryPermit;

        bool isCitizen = npc.nationality == dayManager.HomeCountry;

        bool permitRequired = dayManager.HasDirective(DirectiveType.ForeignersRequireEntryPermit) && !isCitizen;

        if (permit == null)
        {
            discrepancies.Add("Entry permit is missing");
            return;
        }

        if (permit.FullName != npc.FullName)
        {
            discrepancies.Add("Entry permit name does not match");
        }

        if (permit.passportNumber != npc.passportNumber)
        {
            discrepancies.Add("Entry permit passport number does not match");
        }

        if (permit.purpose != npc.entryPurpose)
        {
            discrepancies.Add("Entry puurpose does not match.");
        }

        if (permit.entryByDate.IsExpired(currentDate))
        {
            discrepancies.Add("Enter by date has passed.");
        }

        if (!permit.hasOfficialMark)
        {
            discrepancies.Add("Entry permit seal is incorrect or missing");
        }
    }

    private void VerifySupportingDocument(NPCData npc, List<string> discrepancies)
    {
        if (npc.supportingDocument == null)
        {
            if (dayManager.HasDirective(DirectiveType.SupportingDocumentRequired))
            {
                discrepancies.Add("Supporting document is missing.");
            }

            return;
        }

        if (npc.supportingDocument.HolderName != npc.FullName)
        {
            discrepancies.Add("Supporting document name does not match.");
        }

        switch (npc.supportingDocument)
        {
            case ClearanceCertificateData clearance:
                VerifyClearanceCertificate(clearance, discrepancies);
                break;

            case VaccineCertificationData vaccine:
                VerifyVaccineCertificate(vaccine, discrepancies);
                break;
        }
    }

    private void VerifyClearanceCertificate(ClearanceCertificateData certificate, List<string> discrepancies)
    {
        if (!certificate.hasOfficialMark)
        {
            discrepancies.Add("Clearance certificate seal incorrect or missing");
        }

        if (certificate.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add("Clearance certificate is expired");
        }
    }

    private void VerifyVaccineCertificate(VaccineCertificationData certificate, List<string> discrepancies)
    {
        if(!certificate.hasOfficialMark)
        {
            discrepancies.Add("Vaccine certificate seal is missing or incorrect.");
        }

        if (certificate.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add("Vaccine certificate is expired.");
        }

        if (certificate.exposureStatus == ExposureStatus.Suspected)
        {
            discrepancies.Add("Applicant has suspected skinwalker exposure.");
        }

        if (certificate.exposureStatus == ExposureStatus.Confirmed)
        {
            discrepancies.Add("Applicant has confirmed skinwalker exposure.");
        }
    }

    private void VerifyEntryEligibility(NPCData npc, List<string> discrepancies)
    {
        bool isCitizen = npc.nationality == dayManager.HomeCountry;

        if (dayManager.HasDirective(DirectiveType.CitizensOnly) && !isCitizen)
        {
            discrepancies.Add("Foreign citizens are not permitted today.");
        }
    }
}
