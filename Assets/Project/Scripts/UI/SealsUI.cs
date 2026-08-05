using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sits on the seal grid item prefab - one instance per seal in
/// Rulebook.validSeals, populated at runtime by RulebookUI.
/// </summary>
public class SealItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    public void SetData(Sprite icon, string sealName)
    {
        if (iconImage != null) iconImage.sprite = icon;
        if (nameText != null) nameText.text = sealName;
    }
}
