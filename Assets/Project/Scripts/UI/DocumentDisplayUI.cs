using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] private TMP_Text firstNamePassportText;
        [SerializeField] private TMP_Text lastNamePassportText;
        [SerializeField] private TMP_Text passportCountryText;
        [SerializeField] private TMP_Text passportDobText;
        [SerializeField] private TMP_Text passportSexText;
        [SerializeField] private TMP_Text passportNumberText;
        [SerializeField] private TMP_Text passportExpiryText;
        [SerializeField] private TMP_Text passportIssuingCityText;
        [SerializeField] private Image passportPortraitImage;
        [SerializeField] private Image officialPassportMarkImage;

        [Header("ID Document UI Panel")]
        [SerializeField] private GameObject idPanel;
        [SerializeField] private TMP_Text firstNameIDText;
        [SerializeField] private TMP_Text lastNameIDText;
        [SerializeField] private TMP_Text districtIssuedText;
        [SerializeField] private TMP_Text idDobText;
        [SerializeField] private Image idPortraitImage;

        [Header("Work Permit UI Panel")]
        [SerializeField] private GameObject workPermitPanel;
        [SerializeField] private TMP_Text workPanelHolderNameText;
        [SerializeField] private TMP_Text workFieldText;
        [SerializeField] private TMP_Text validUntilDate;


        [Header("Entry Permit UI Panel")]
        [SerializeField] private GameObject entryPermitPanel;
        [SerializeField] private TMP_Text entryPermitFirstNameText;
        [SerializeField] private TMP_Text entryPermitLastNameText;
        [SerializeField] private TMP_Text entryPermitPassportNumText;
        [SerializeField] private TMP_Text entryPermitPurposeText;
        [SerializeField] private TMP_Text entryPermitEntryByDateText;
        [SerializeField] private TMP_Text stayDurationText;
        [SerializeField] private Image entryPermitSealGraphic;

        [Header("Clearance Certificate UI Panel")]
        [SerializeField] private GameObject clearanceDocPanel;
        [SerializeField] private TMP_Text clearanceFirstNameText;
        [SerializeField] private TMP_Text clearanceLastNameText;
        [SerializeField] private TMP_Text clearenceDobText;
        [SerializeField] private TMP_Text clearanceCountryText;
        [SerializeField] private Image clearancePortraitImage;
        [SerializeField] private TMP_Text offenceText;
        [SerializeField] private TMP_Text clearanceIssueDateText;
        [SerializeField] private TMP_Text clearanceValidUntilText;
        [SerializeField] private TMP_Text offenceCategoryText;
        [SerializeField] private Image offenceMarkImage;


        [Header("Vaccine Ceritificate UI Panel")]
        [SerializeField] private GameObject vaccineDocPanel;
        [SerializeField] private TMP_Text vaccineFirstNameText;
        [SerializeField] private TMP_Text vaccineLastNameText;
        [SerializeField] private TMP_Text vaccineDobText;
        [SerializeField] private TMP_Text vaccineTypeText;
        [SerializeField] private TMP_Text vaccineDosesText;
        [SerializeField] private TMP_Text exposureStatusText;
        [SerializeField] private TMP_Text vaccineIssueDateText;
        [SerializeField] private TMP_Text vaccineValidUntilText;
        [SerializeField] private TMP_Text medicalFacilityText;
        [SerializeField] private Image vaccineOfficialMark;


        /// <summary>
        /// Update desk UI documents for a new incoming NPC.
        /// </summary>
        public void DisplayNPCDocuments(NPCData npc)
        {
            Debug.Log(
        npc == null
            ? "[DocumentDisplayUI] NPC received was null."
            : $"[DocumentDisplayUI] Received NPC: {npc.FullName}"
    );
            ClearAllDocuments();

            if (npc == null)
            {
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
            ClearAllDocuments();

            if (passportPanel == null) return;
            PassportDocumentData passport = npc.passportDocument;

            if (passport == null)
            {
                passportPanel.SetActive(false);
                return;
            }

            passportPanel.SetActive(true);

            if (firstNamePassportText != null) firstNamePassportText.text = passport.firstName.ToString();

            if (lastNamePassportText != null) lastNamePassportText.text = passport.lastName.ToString();

            if (passportCountryText != null) passportCountryText.text = passport.country.ToString();

            if (passportDobText != null) passportDobText.text =passport.dateOfBirth.ToString();

            if (passportSexText != null) passportSexText.text = passport.sex.ToString();

            if (passportNumberText != null) passportNumberText.text = passport.passportNumber;

            if (passportExpiryText != null) passportExpiryText.text = passport.expirationDate.ToString();

            if (passportIssuingCityText != null) passportIssuingCityText.text = passport.issuingCity.ToString();

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

            if (firstNameIDText != null) firstNameIDText.text = idCard.firstName;

            if (lastNameIDText != null) lastNameIDText.text = idCard.lastName;

            if (districtIssuedText != null) districtIssuedText.text = idCard.districtIssued.ToString();

            if (idDobText != null) idDobText.text = idCard.dateOfBirth.ToString();


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

            if (workPanelHolderNameText != null) workPanelHolderNameText.text = workPermit.HolderName.ToString();

            if (workFieldText != null) workFieldText.text = workPermit.workField.ToString();

            if (validUntilDate != null) validUntilDate.text = workPermit.validUntil.ToString();
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

            if (entryPermitFirstNameText != null) entryPermitFirstNameText.text = permit.firstName.ToString();

            if (entryPermitLastNameText != null) entryPermitLastNameText.text = permit.lastName.ToString();

            if (entryPermitPassportNumText != null) entryPermitPassportNumText.text = permit.passportNumber.ToString();

            if (entryPermitPurposeText != null) entryPermitPurposeText.text = permit.purpose.ToString();

            if (entryPermitEntryByDateText != null) entryPermitEntryByDateText.text = permit.entryByDate.ToString();

            if (stayDurationText != null) stayDurationText.text = $"{permit.durationDays} days";

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

            if (vaccineFirstNameText != null) vaccineFirstNameText.text = vaccine.firstName;

            if (vaccineLastNameText != null) vaccineLastNameText.text = vaccine.lastName;

            if (vaccineDobText != null) vaccineDobText.text =   vaccine.dateOfBirth.ToString();

            if (vaccineTypeText != null) vaccineTypeText.text = vaccine.vaccineType.ToString();

            if (vaccineDosesText != null) vaccineDosesText.text = vaccine.doses.ToString();

            if (exposureStatusText != null) exposureStatusText.text = vaccine.exposureStatus.ToString();

            if (vaccineIssueDateText != null) vaccineIssueDateText.text = vaccine.issueDate.ToString();

            if (vaccineValidUntilText != null) vaccineValidUntilText.text = vaccine.validUntil.ToString();

            if (medicalFacilityText != null) medicalFacilityText.text = vaccine.facility.ToString();
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
                clearanceFirstNameText.text = clearance.firstName.ToString();
            }

            if (clearanceLastNameText != null)
            {
                clearanceLastNameText.text = clearance.lastName.ToString();
            }

            if (clearenceDobText != null)
            {
                clearenceDobText.text = clearance.dateOfBirth.ToString();
            }

            if (clearanceCountryText != null)
            {
                clearanceCountryText.text = clearance.nationality.ToString();
            }

            if (clearancePortraitImage != null)
            {
                clearancePortraitImage.sprite = clearance.Photo;

                clearancePortraitImage.enabled = clearance.Photo != null;
            }

            if (clearanceIssueDateText != null)
            {
                clearanceIssueDateText.text = clearance.issueDate.ToString();
            }

            if (clearanceValidUntilText != null)
            {
                clearanceValidUntilText.text = clearance.validUntil.ToString();
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
