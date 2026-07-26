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
                    if (passportCountryText != null) passportCountryText.text = passport.country;
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
                    if (idCountryText != null) idCountryText.text = idDoc.country;
                    if (idDobText != null) idDobText.text = idDoc.dateOfBirth.ToString();
                    if (idHeightText != null) idHeightText.text = $"{idDoc.heightCm} cm";
                    if (idWeightText != null) idWeightText.text = $"{idDoc.weightKg} kg";
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

#if UNITY_EDITOR
        [ContextMenu("Auto-Build All Document Panels")]
        public void AutoBuildDocumentPanels()
        {
            Transform parentCanvas = transform;

            // Ensure DocumentsContainer exists
            Transform docsContainer = parentCanvas.Find("DocumentsContainer");
            if (docsContainer == null)
            {
                GameObject containerObj = new GameObject("DocumentsContainer", typeof(RectTransform));
                containerObj.transform.SetParent(parentCanvas, false);
                RectTransform rt = containerObj.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                docsContainer = containerObj.transform;
            }

            Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Build Passport Panel
            passportPanel = CreateOrGetPanel(docsContainer, "PassportPanel", new Vector2(-200, 50), new Vector2(300, 420), new Color(0.2f, 0.15f, 0.1f, 0.95f));
            passportHolderNameText = CreateOrGetText(passportPanel.transform, "PassportHolderName", "Name: ---", new Vector2(10, -20), defaultFont, 16);
            passportCountryText = CreateOrGetText(passportPanel.transform, "PassportCountry", "Country: ---", new Vector2(10, -50), defaultFont, 14);
            passportDobText = CreateOrGetText(passportPanel.transform, "PassportDOB", "DOB: ---", new Vector2(10, -80), defaultFont, 14);
            passportSexText = CreateOrGetText(passportPanel.transform, "PassportSex", "Sex: ---", new Vector2(10, -110), defaultFont, 14);
            passportNumberText = CreateOrGetText(passportPanel.transform, "PassportNumber", "No: ---", new Vector2(10, -140), defaultFont, 14);
            passportExpiryText = CreateOrGetText(passportPanel.transform, "PassportExpiry", "Exp: ---", new Vector2(10, -170), defaultFont, 14);
            passportPortraitImage = CreateOrGetImage(passportPanel.transform, "PassportPortrait", new Vector2(10, -210), new Vector2(100, 120));

            // Build ID Panel
            idPanel = CreateOrGetPanel(docsContainer, "IDPanel", new Vector2(150, 80), new Vector2(280, 220), new Color(0.15f, 0.2f, 0.25f, 0.95f));
            idHolderNameText = CreateOrGetText(idPanel.transform, "IDHolderName", "Name: ---", new Vector2(10, -20), defaultFont, 16);
            idCountryText = CreateOrGetText(idPanel.transform, "IDCountry", "Country: ---", new Vector2(10, -50), defaultFont, 14);
            idDobText = CreateOrGetText(idPanel.transform, "IDDOB", "DOB: ---", new Vector2(10, -80), defaultFont, 14);
            idHeightText = CreateOrGetText(idPanel.transform, "IDHeight", "Height: ---", new Vector2(10, -110), defaultFont, 14);
            idWeightText = CreateOrGetText(idPanel.transform, "IDWeight", "Weight: ---", new Vector2(10, -140), defaultFont, 14);

            // Build Entry Permit Panel
            entryPermitPanel = CreateOrGetPanel(docsContainer, "EntryPermitPanel", new Vector2(-150, -150), new Vector2(320, 200), new Color(0.25f, 0.25f, 0.2f, 0.95f));
            entryPermitNameText = CreateOrGetText(entryPermitPanel.transform, "PermitName", "Name: ---", new Vector2(10, -20), defaultFont, 16);
            entryPermitPassportNumText = CreateOrGetText(entryPermitPanel.transform, "PermitPassNo", "Pass #: ---", new Vector2(10, -50), defaultFont, 14);
            entryPermitPurposeText = CreateOrGetText(entryPermitPanel.transform, "PermitPurpose", "Purpose: ---", new Vector2(10, -80), defaultFont, 14);
            entryPermitEntryByDateText = CreateOrGetText(entryPermitPanel.transform, "PermitEntryBy", "Entry By: ---", new Vector2(10, -110), defaultFont, 14);
            entryPermitSealGraphic = CreateOrGetPanel(entryPermitPanel.transform, "OfficialSealGraphic", new Vector2(220, -120), new Vector2(60, 60), new Color(0.8f, 0.2f, 0.2f, 0.8f));

            // Build Supporting Doc Panel
            supportingDocPanel = CreateOrGetPanel(docsContainer, "SupportingDocPanel", new Vector2(180, -120), new Vector2(300, 200), new Color(0.2f, 0.25f, 0.2f, 0.95f));
            supportingDocTitleText = CreateOrGetText(supportingDocPanel.transform, "DocTitle", "CERTIFICATE", new Vector2(10, -20), defaultFont, 16);
            supportingDocHolderNameText = CreateOrGetText(supportingDocPanel.transform, "DocHolderName", "Name: ---", new Vector2(10, -50), defaultFont, 14);
            supportingDocStatusText = CreateOrGetText(supportingDocPanel.transform, "DocStatus", "Status: ---", new Vector2(10, -80), defaultFont, 14);
            supportingDocExpiryText = CreateOrGetText(supportingDocPanel.transform, "DocExpiry", "Exp: ---", new Vector2(10, -110), defaultFont, 14);
            supportingDocSealGraphic = CreateOrGetPanel(supportingDocPanel.transform, "SuppSealGraphic", new Vector2(200, -120), new Vector2(50, 50), new Color(0.2f, 0.6f, 0.8f, 0.8f));

            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("[DocumentDisplayUI] Successfully auto-built and wired up all 4 Document UI panels!");
        }

        private GameObject CreateOrGetPanel(Transform parent, string name, Vector2 anchoredPos, Vector2 size, Color color)
        {
            Transform child = parent.Find(name);
            GameObject obj;
            if (child == null)
            {
                obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup), typeof(DraggableDocument));
                obj.transform.SetParent(parent, false);
            }
            else
            {
                obj = child.gameObject;
                if (!obj.GetComponent<CanvasGroup>()) obj.AddComponent<CanvasGroup>();
                if (!obj.GetComponent<DraggableDocument>()) obj.AddComponent<DraggableDocument>();
            }

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = size;

            Image img = obj.GetComponent<Image>();
            if (img != null) img.color = color;

            return obj;
        }

        private Text CreateOrGetText(Transform parent, string name, string defaultText, Vector2 localPos, Font font, int fontSize)
        {
            Transform child = parent.Find(name);
            GameObject obj;
            if (child == null)
            {
                obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
                obj.transform.SetParent(parent, false);
            }
            else
            {
                obj = child.gameObject;
            }

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = localPos;
            rt.sizeDelta = new Vector2(260, 28);

            Text txt = obj.GetComponent<Text>();
            txt.text = defaultText;
            txt.font = font;
            txt.fontSize = fontSize;
            txt.color = Color.white;
            txt.alignment = TextAnchor.MiddleLeft;

            return txt;
        }

        private Image CreateOrGetImage(Transform parent, string name, Vector2 localPos, Vector2 size)
        {
            Transform child = parent.Find(name);
            GameObject obj;
            if (child == null)
            {
                obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                obj.transform.SetParent(parent, false);
            }
            else
            {
                obj = child.gameObject;
            }

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = localPos;
            rt.sizeDelta = size;

            return obj.GetComponent<Image>();
        }
#endif
    }
}
