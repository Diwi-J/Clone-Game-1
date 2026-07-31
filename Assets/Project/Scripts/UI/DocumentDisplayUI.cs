using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    /// <summary>
    /// Binds an active NPC's data and presented documents to visual UI card components.
    /// Handles populating field labels (name, passport #, DOB, dates, seals, photo).
    /// </summary>
    public class DocumentDisplayUI : MonoBehaviour
    {
        [Header("Passport UI Panel")]
        [SerializeField] private GameObject passportPanel;
        [SerializeField] private Text firstNamePassportText;
        [SerializeField] private Text lastNamePassportText;
        [SerializeField] private Text passportCountryText;
        [SerializeField] private Text passportDobText;
        [SerializeField] private Text passportSexText;
        [SerializeField] private Text passportNumberText;
        [SerializeField] private Text passportExpiryText;
        [SerializeField] private Image passportPortraitImage;
        [SerializeField] private Image officialPassportMarkImage;

        [Header("ID Document UI Panel")]
        [SerializeField] private GameObject idPanel;
        [SerializeField] private Text firstNameIDText;
        [SerializeField] private Text lastNameIDText;
        [SerializeField] private Text districtIssuedText;
        [SerializeField] private Text idCountryText;
        [SerializeField] private Text idDobText;
        [SerializeField] private Text idNumberText;
        [SerializeField] private Image idPortraitImage;

        [Header("Work Permit UI Panel")]
        [SerializeField] private GameObject workPermitPanel;
        [SerializeField] private Text workPanelHolderNameText;
        [SerializeField] private Text workFieldText;
        [SerializeField] private Text validUntilDate;


        [Header("Entry Permit UI Panel")]
        [SerializeField] private GameObject entryPermitPanel;
        [SerializeField] private Text entryPermitFirstNameText;
        [SerializeField] private Text entryPermitLastNameText;
        [SerializeField] private Text entryPermitPassportNumText;
        [SerializeField] private Text entryPermitPurposeText;
        [SerializeField] private Text entryPermitEntryByDateText;
        [SerializeField] private Text stayDurationText;
        [SerializeField] private Image entryPermitSealGraphic;

        [Header("Clearance Certificate UI Panel")]
        [SerializeField] private GameObject clearanceDocPanel;
        [SerializeField] private Text clearanceFirstNameText;
        [SerializeField] private Text clearanceLastNameText;
        [SerializeField] private Text clearenceDobText;
        [SerializeField] private Text clearanceCountryText;
        [SerializeField] private Image clearancePortraitImage;
        [SerializeField] private Text offenceText;
        [SerializeField] private Text clearanceIssueDateText;
        [SerializeField] private Text clearanceValidUntilText;
        [SerializeField] private Text offenceCategoryText;
        [SerializeField] private Image offenceMarkImage;


        [Header("Vaccine Ceritificate UI Panel")]
        [SerializeField] private GameObject vaccineDocPanel;
        [SerializeField] private Text vaccineFirstNameText;
        [SerializeField] private Text vaccineLastNameText;
        [SerializeField] private Text vaccineDobText;
        [SerializeField] private Text vaccineTypeText;
        [SerializeField] private Text vaccineDosesText;
        [SerializeField] private Text exposureStatusText;
        [SerializeField] private Text vaccineIssueDateText;
        [SerializeField] private Text vaccineValidUntilText;
        [SerializeField] private Text medicalFacilityText;


        /// <summary>
        /// Update desk UI documents for a new incoming NPC.
        /// </summary>
        public void DisplayNPCDocuments(NPCData npc)
        {
            if (npc == null)
            {
                ClearAllDocuments();
                return;
            }

            SetupPassport(npc);
            SetupIDCard(npc);
            SetupWorkPermit(npc);
            SetupEntryPermit(npc);
            SetupClearanceCertificate(npc);
            SetupVaccineCertificate(npc);
        }

        public void ClearAllDocuments()
        {
            if (passportPanel != null) passportPanel.SetActive(false);

            if (idPanel != null) idPanel.SetActive(false);

            if (workPermitPanel != null) workPermitPanel.SetActive(false);

            if (entryPermitPanel != null) entryPermitPanel.SetActive(false);

            if (clearanceDocPanel != null) clearanceDocPanel.SetActive(false);

            if (vaccineDocPanel != null) vaccineDocPanel.SetActive(false);
        }

        private void SetupPassport(NPCData npc)
        {
            if (passportPanel == null) return;
            PassportDocumentData passport = npc.passportDocument;

            if (passport == null)
            {
                passportPanel.SetActive(false);
                return;
            }

            passportPanel.SetActive(true);

            if (firstNamePassportText != null) firstNamePassportText.text = $"First Name: {passport.firstName}";

            if (lastNamePassportText != null) lastNamePassportText.text = $"Last Name: {passport.lastName}";

            if (passportCountryText != null) passportCountryText.text = $"Country: {passport.country}";

            if (passportDobText != null) passportDobText.text = $"Date of Birth: {passport.dateOfBirth}";

            if (passportSexText != null) passportSexText.text = $"Sex: {passport.sex}";

            if (passportNumberText != null) passportNumberText.text = $"Passport Number: {passport.passportNumber}";

            if (passportExpiryText != null) passportExpiryText.text = $"Expiry Date: {passport.expirationDate}";

            if (passportPortraitImage != null)
            {
                passportPortraitImage.sprite = passport.portrait;
                passportPortraitImage.enabled =
                    passport.portrait != null;
            }

            if (officialPassportMarkImage != null)
            {
                officialPassportMarkImage.enabled =
                    passport.hasOfficialMark;
            }
        }

        private void SetupIDCard(NPCData npc)
        {
            if (idPanel == null) return;

            IDDocumentData idCard = npc.identificationDocument;

            if (idCard == null)
            {
                idPanel.SetActive(false);
                return;
            }

            idPanel.SetActive(true);

            if (firstNameIDText != null) firstNameIDText.text = $"First Name: {idCard.firstName}";

            if (lastNameIDText != null) lastNameIDText.text = $"Last Name: {idCard.lastName}";

            if (districtIssuedText != null) districtIssuedText.text = $"District Issued: {idCard.districtIssued}";

            if (idCountryText != null) idCountryText.text = $"Country: {idCard.country}";

            if (idDobText != null) idDobText.text = $"Date of Birth: {idCard.dateOfBirth}";

            if (idNumberText != null) idNumberText.text = $"ID Number: {idCard.idNumber}";

            if (idPortraitImage != null)
            {
                idPortraitImage.sprite = idCard.portrait;
                idPortraitImage.enabled =
                    idCard.portrait != null;
            }
        }

        private void SetupWorkPermit(NPCData npc)
        {
            if (workPermitPanel == null)
                return;

            WorkPermitDocumentData workPermit = npc.workPermit;

            if (workPermit == null)
            {
                workPermitPanel.SetActive(false);
                return;
            }

            workPermitPanel.SetActive(true);

            if (workPanelHolderNameText != null) workPanelHolderNameText.text = $"Holder: {workPermit.HolderName}";

            if (workFieldText != null) workFieldText.text = $"Work Field: {workPermit.workField}";

            if (validUntilDate != null) validUntilDate.text = $"Valid Until: {workPermit.validUntil}";
        }

        private void SetupEntryPermit(NPCData npc)
        {
            if (entryPermitPanel == null)
                return;

            EntryPermitData permit = npc.entryPermit;

            if (permit == null)
            {
                entryPermitPanel.SetActive(false);
                return;
            }

            entryPermitPanel.SetActive(true);

            if (entryPermitFirstNameText != null) entryPermitFirstNameText.text = $"First Name: {permit.firstName}";

            if (entryPermitLastNameText != null) entryPermitLastNameText.text = $"Last Name: {permit.lastName}";

            if (entryPermitPassportNumText != null) entryPermitPassportNumText.text = $"Passport Number: {permit.passportNumber}";

            if (entryPermitPurposeText != null) entryPermitPurposeText.text = $"Purpose: {permit.purpose}";

            if (entryPermitEntryByDateText != null) entryPermitEntryByDateText.text = $"Entry By: {permit.entryByDate}";

            if (stayDurationText != null) stayDurationText.text = $"Duration: {permit.durationDays} days";

            if (entryPermitSealGraphic != null)
            {
                entryPermitSealGraphic.enabled =
                    permit.hasOfficialMark;
            }
        }

        private void SetupVaccineCertificate(NPCData npc)
        {
            if (vaccineDocPanel == null)
                return;

            VaccineCertificationData vaccine =
                npc.vaccineCertification;

            if (vaccine == null)
            {
                vaccineDocPanel.SetActive(false);
                return;
            }

            vaccineDocPanel.SetActive(true);

            if (vaccineFirstNameText != null) vaccineFirstNameText.text = $"First Name: {vaccine.firstName}";

            if (vaccineLastNameText != null) vaccineLastNameText.text = $"Last Name: {vaccine.lastName}";

            if (vaccineDobText != null) vaccineDobText.text = $"Date of Birth: {vaccine.dateOfBirth}";

            if (vaccineTypeText != null) vaccineTypeText.text = $"Vaccine Type: {vaccine.vaccineType}";

            if (vaccineDosesText != null) vaccineDosesText.text = $"Doses: {vaccine.doses}";

            if (exposureStatusText != null) exposureStatusText.text = $"Exposure: {vaccine.exposureStatus}";

            if (vaccineIssueDateText != null) vaccineIssueDateText.text = $"Issue Date: {vaccine.issueDate}";

            if (vaccineValidUntilText != null) vaccineValidUntilText.text = $"Valid Until: {vaccine.validUntil}";

            if (medicalFacilityText != null) medicalFacilityText.text = $"Medical Facility: {vaccine.facility}";
        }
    

    private void SetupClearanceCertificate(NPCData npc)
        {
            if (clearanceDocPanel == null)
                return;

            ClearanceCertificateData clearance =
                npc.clearanceDocument;

            if (clearance == null)
            {
                clearanceDocPanel.SetActive(false);
                return;
            }

            clearanceDocPanel.SetActive(true);

            if (clearanceFirstNameText != null)
            {
                clearanceFirstNameText.text = $"First Name: {clearance.firstName}";
            }

            if (clearanceLastNameText != null)
            {
                clearanceLastNameText.text = $"Last Name: {clearance.lastName}";
            }

            if (clearenceDobText != null)
            {
                clearenceDobText.text = $"Date of Birth: {clearance.dateOfBirth}";
            }

            if (clearanceCountryText != null)
            {
                clearanceCountryText.text = $"Nationality: {clearance.nationality}";
            }

            if (clearancePortraitImage != null)
            {
                clearancePortraitImage.sprite = clearance.Photo;

                clearancePortraitImage.enabled = clearance.Photo != null;
            }

            if (clearanceIssueDateText != null)
            {
                clearanceIssueDateText.text = $"Issue Date: {clearance.issueDate}";
            }

            if (clearanceValidUntilText != null)
            {
                clearanceValidUntilText.text = $"Valid Until: {clearance.validUntil}";
            }

            if (offenceText != null)
            {
                if (clearance.offences == null || clearance.offences.Count == 0)
                {
                    offenceText.text = "Offences: None";
                }
                else
                {
                    string offenceList = "Offences:\n";

                    foreach (OffenceData offence in clearance.offences)
                    {
                        offenceList += $"- {offence.offenceName}\n";
                    }

                    offenceText.text = offenceList;
                }
            }

            if (offenceCategoryText != null)
            {
                if (clearance.offences == null ||
                    clearance.offences.Count == 0)
                {
                    offenceCategoryText.text = "Categories: None";
                }
                else
                {
                    string categoryList = "Categories:\n";

                    foreach (OffenceData offence in clearance.offences)
                    {
                        categoryList += $"- {offence.category}\n";
                    }

                    offenceCategoryText.text = categoryList;
                }
            }

            if (offenceMarkImage != null)
            {
                offenceMarkImage.enabled = clearance.hasOfficialMark;
            }
        }
    }
}
