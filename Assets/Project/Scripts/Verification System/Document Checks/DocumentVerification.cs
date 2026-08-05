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
            discrepancies.Add("No NPC data was provided.");
            return discrepancies;
        }

        if (dayManager == null)
        {
            discrepancies.Add("DayManager reference is missing.");
            return discrepancies;
        }

        VerifyEntryEligibility(npc, discrepancies);

        VerifyPassport(npc, discrepancies);
        VerifyIDCard(npc, discrepancies);
        VerifyEntryPermit(npc, discrepancies);
        VerifyWorkPermit(npc, discrepancies);
        VerifyClearanceCertificate(npc, discrepancies);
        VerifyVaccineCertificate(npc, discrepancies);

        return discrepancies;
    }

    // PASSPORT
    private void VerifyPassport(
        NPCData npc,
        List<string> discrepancies)
    {
        PassportDocumentData passport = npc.passportDocument;

        // Everyone must have a passport because it is stamped.
        if (passport == null)
        {
            discrepancies.Add("Passport is missing.");
            return;
        }

        if (passport.HolderName != npc.FullName)
        {
            discrepancies.Add("Passport name does not match.");
        }

        if (passport.isForged)
        {
            discrepancies.Add("Passport is forged.");
        }

        if (
            passport.dateOfBirth.ToDateTime() !=
            npc.dateOfBirth.ToDateTime()
        )
        {
            discrepancies.Add(
                "Passport date of birth does not match."
            );
        }

        if (passport.country != npc.nationality)
        {
            discrepancies.Add(
                "Passport nationality does not match."
            );
        }

        if (passport.sex != npc.sex)
        {
            discrepancies.Add("Passport sex does not match.");
        }

        if (passport.passportNumber != npc.passportNumber)
        {
            discrepancies.Add(
                "Passport number does not match."
            );
        }

        if (passport.expirationDate.IsExpired(currentDate))
        {
            discrepancies.Add("Passport is expired.");
        }
    }

    // ID CARD
    private void VerifyIDCard(
        NPCData npc,
        List<string> discrepancies)
    {
        IDDocumentData idCard = npc.identificationDocument;

        // ID Cards are optional unless you create an ID-required rule.
        if (idCard == null)
        {
            return;
        }
        bool isCitizen =
        npc.nationality == dayManager.HomeCountry;

        bool idRequired =
       isCitizen &&
       dayManager.HasDirective(
           DirectiveType.IdentificationRequired
       );

        if (idCard.HolderName != npc.FullName)
        {
            discrepancies.Add("ID Card name does not match.");
        }

        if (idCard.isForged)
        {
            discrepancies.Add("ID Card is forged.");
        }

        if (
            idCard.dateOfBirth.ToDateTime() !=
            npc.dateOfBirth.ToDateTime()
        )
        {
            discrepancies.Add(
                "ID Card date of birth does not match."
            );
        }


    }


    // ENTRY PERMIT
    private void VerifyEntryPermit(
        NPCData npc,
        List<string> discrepancies)
    {
        bool isCitizen =
            npc.nationality == dayManager.HomeCountry;

        bool permitRequired =
            dayManager.HasDirective(
                DirectiveType.ForeignersRequireEntryPermit
            )
            && !isCitizen;

        EntryPermitData permit = npc.entryPermit;

        if (permit == null)
        {
            // Only report it as missing when it is required.
            if (permitRequired)
            {
                discrepancies.Add("Entry Permit is missing.");
            }

            return;
        }

        if (permit.FullName != npc.FullName)
        {
            discrepancies.Add(
                "Entry Permit name does not match."
            );
        }

        if (permit.passportNumber != npc.passportNumber)
        {
            discrepancies.Add(
                "Entry Permit passport number does not match."
            );
        }

        if (permit.purpose != npc.entryPurpose)
        {
            discrepancies.Add(
                "Entry Permit purpose does not match."
            );
        }

        if (permit.entryByDate.IsExpired(currentDate))
        {
            discrepancies.Add(
                "Entry Permit entry-by date has passed."
            );
        }

        if (!permit.hasOfficialMark)
        {
            discrepancies.Add(
                "Entry Permit seal is incorrect or missing."
            );
        }

        if (permit.isForged)
        {
            discrepancies.Add("Entry Permit is forged.");
        }
    }

    // WORK PERMIT
    private void VerifyWorkPermit(
        NPCData npc,
        List<string> discrepancies)
    {
        WorkPermitDocumentData workPermit = npc.workPermit;

        bool workPermitRequired =
            npc.entryPurpose == EntryPurpose.Work;

        if (workPermit == null)
        {
            if (workPermitRequired)
            {
                discrepancies.Add("Work Permit is missing.");
            }

            return;
        }

        if (workPermit.HolderName != npc.FullName)
        {
            discrepancies.Add(
                "Work Permit name does not match."
            );
        }

        if (workPermit.isForged)
        {
            discrepancies.Add("Work Permit is forged.");
        }

        if (!workPermit.hasOfficialMark)
        {
            discrepancies.Add(
                "Work Permit seal is incorrect or missing."
            );
        }

        if (workPermit.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add("Work Permit is expired.");
        }
    }

    // CLEARANCE CERTIFICATE
    private void VerifyClearanceCertificate(
        NPCData npc,
        List<string> discrepancies)
    {
        ClearanceCertificateData clearance =
            npc.clearanceDocument;

        bool supportingDocumentRequired =
            dayManager.HasDirective(
                DirectiveType.SupportingDocumentRequired
            );

        // The applicant may satisfy the supporting-document
        // requirement with either clearance or vaccine.
        if (
            clearance == null &&
            npc.vaccineCertification == null
        )
        {
            if (supportingDocumentRequired)
            {
                discrepancies.Add(
                    "Supporting document is missing."
                );
            }

            return;
        }

        // No clearance was presented, but a vaccine may exist.
        if (clearance == null)
        {
            return;
        }

        if (clearance.HolderName != npc.FullName)
        {
            discrepancies.Add(
                "Clearance Certificate name does not match."
            );
        }

        if (clearance.isForged)
        {
            discrepancies.Add(
                "Clearance Certificate is forged."
            );
        }

        if (!clearance.hasOfficialMark)
        {
            discrepancies.Add(
                "Clearance Certificate seal is incorrect or missing."
            );
        }

        if (clearance.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add(
                "Clearance Certificate is expired."
            );
        }
    }

    // VACCINE CERTIFICATE
    private void VerifyVaccineCertificate(
        NPCData npc,
        List<string> discrepancies)
    {
        VaccineCertificationData vaccine =
            npc.vaccineCertification;

        if (vaccine == null)
        {
            return;
        }

        if (vaccine.HolderName != npc.FullName)
        {
            discrepancies.Add(
                "Vaccine Certificate name does not match."
            );
        }

        if (vaccine.isForged)
        {
            discrepancies.Add(
                "Vaccine Certificate is forged."
            );
        }

        if (!vaccine.hasOfficialMark)
        {
            discrepancies.Add(
                "Vaccine Certificate seal is incorrect or missing."
            );
        }

        if (vaccine.validUntil.IsExpired(currentDate))
        {
            discrepancies.Add(
                "Vaccine Certificate is expired."
            );
        }

        if (
            vaccine.exposureStatus ==
            ExposureStatus.Suspected
        )
        {
            discrepancies.Add(
                "Applicant has suspected Skinwalker exposure."
            );
        }

        if (
            vaccine.exposureStatus ==
            ExposureStatus.Confirmed
        )
        {
            discrepancies.Add(
                "Applicant has confirmed Skinwalker exposure."
            );
        }
    }

    // ENTRY ELIGIBILITY
    private void VerifyEntryEligibility(
        NPCData npc,
        List<string> discrepancies)
    {
        bool isCitizen =
            npc.nationality == dayManager.HomeCountry;

        if (
            dayManager.HasDirective(
                DirectiveType.CitizensOnly
            )
            && !isCitizen
        )
        {
            discrepancies.Add(
                "Foreigners are not permitted today."
            );
        }
    }
}
