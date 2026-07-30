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
        [SerializeField] private Text passportHolderNameText;
        [SerializeField] private Text passportCountryText;
        [SerializeField] private Text passportDobText;
        [SerializeField] private Text passportSexText;
        [SerializeField] private Text passportNumberText;
        [SerializeField] private Text passportExpiryText;
        [SerializeField] private Image passportPortraitImage;

        [Header("ID Document UI Panel")]
        [SerializeField] private GameObject idPanel;
        [SerializeField] private Text idHolderNameText;
        [SerializeField] private Text idCountryText;
        [SerializeField] private Text idDobText;
        [SerializeField] private Text idHeightText;
        [SerializeField] private Text idWeightText;

        [Header("Entry Permit UI Panel")]
        [SerializeField] private GameObject entryPermitPanel;
        [SerializeField] private Text entryPermitNameText;
        [SerializeField] private Text entryPermitPassportNumText;
        [SerializeField] private Text entryPermitPurposeText;
        [SerializeField] private Text entryPermitEntryByDateText;
        [SerializeField] private GameObject entryPermitSealGraphic;

        [Header("Supporting Document UI Panel")]
        [SerializeField] private GameObject supportingDocPanel;
        [SerializeField] private Text supportingDocTitleText;
        [SerializeField] private Text supportingDocHolderNameText;
        [SerializeField] private Text supportingDocStatusText;
        [SerializeField] private Text supportingDocExpiryText;
        [SerializeField] private GameObject supportingDocSealGraphic;

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

            SetupIdentificationDoc(npc);
            SetupEntryPermit(npc);
            SetupSupportingDoc(npc);
        }

        public void ClearAllDocuments()
        {
            if (passportPanel != null) passportPanel.SetActive(false);
            if (idPanel != null) idPanel.SetActive(false);
            if (entryPermitPanel != null) entryPermitPanel.SetActive(false);
            if (supportingDocPanel != null) supportingDocPanel.SetActive(false);
        }

        private void SetupIdentificationDoc(NPCData npc)
        {
            if (passportPanel != null) passportPanel.SetActive(false);
            if (idPanel != null) idPanel.SetActive(false);

            if (npc.identificationDocument == null) return;

            if (npc.identificationDocument is PassportDocumentData passport)
            {
                if (passportPanel != null)
                {
                    passportPanel.SetActive(true);
                    if (passportHolderNameText != null) passportHolderNameText.text = passport.HolderName;
                    if (passportCountryText != null) passportCountryText.text = passport.country.ToString();
                    if (passportDobText != null) passportDobText.text = passport.dateOfBirth.ToString();
                    if (passportSexText != null) passportSexText.text = passport.sex.ToString();
                    if (passportNumberText != null) passportNumberText.text = passport.passportNumber;
                    if (passportExpiryText != null) passportExpiryText.text = passport.expirationDate.ToString();
                    if (passportPortraitImage != null)
                    {
                        passportPortraitImage.sprite = npc.portrait;
                        passportPortraitImage.enabled = (npc.portrait != null);
                    }
                }
            }
            else if (npc.identificationDocument is IDDocumentData idDoc)
            {
                if (idPanel != null)
                {
                    idPanel.SetActive(true);
                    if (idHolderNameText != null) idHolderNameText.text = idDoc.HolderName;
                    if (idCountryText != null) idCountryText.text = idDoc.country.ToString();
                    if (idDobText != null) idDobText.text = idDoc.dateOfBirth.ToString();
                }
            }
        }

        private void SetupEntryPermit(NPCData npc)
        {
            if (entryPermitPanel == null) return;

            EntryPermitData permit = npc.entryPermit;
            if (permit == null)
            {
                entryPermitPanel.SetActive(false);
                return;
            }

            entryPermitPanel.SetActive(true);
            if (entryPermitNameText != null) entryPermitNameText.text = permit.FullName;
            if (entryPermitPassportNumText != null) entryPermitPassportNumText.text = permit.passportNumber;
            if (entryPermitPurposeText != null) entryPermitPurposeText.text = permit.purpose.ToString();
            if (entryPermitEntryByDateText != null) entryPermitEntryByDateText.text = permit.entryByDate.ToString();
            if (entryPermitSealGraphic != null) entryPermitSealGraphic.SetActive(permit.hasOfficialMark);
        }

        private void SetupSupportingDoc(NPCData npc)
        {
            if (supportingDocPanel == null) return;

            SupportingDocumentData supp = npc.supportingDocument;
            if (supp == null)
            {
                supportingDocPanel.SetActive(false);
                return;
            }

            supportingDocPanel.SetActive(true);
            if (supportingDocHolderNameText != null) supportingDocHolderNameText.text = supp.HolderName;

            if (supp is ClearanceCertificateData clearance)
            {
                if (supportingDocTitleText != null) supportingDocTitleText.text = "CLEARANCE CERTIFICATE";
                if (supportingDocStatusText != null) supportingDocStatusText.text = "Official Clear Status";
                if (supportingDocExpiryText != null) supportingDocExpiryText.text = clearance.validUntil.ToString();
                if (supportingDocSealGraphic != null) supportingDocSealGraphic.SetActive(clearance.hasOfficialMark);
            }
            else if (supp is VaccineCertificationData vaccine)
            {
                if (supportingDocTitleText != null) supportingDocTitleText.text = "VACCINE CERTIFICATE";
                if (supportingDocStatusText != null) supportingDocStatusText.text = $"Exposure: {vaccine.exposureStatus}";
                if (supportingDocExpiryText != null) supportingDocExpiryText.text = vaccine.validUntil.ToString();
                if (supportingDocSealGraphic != null) supportingDocSealGraphic.SetActive(vaccine.hasOfficialMark);
            }
        }
    }
}
