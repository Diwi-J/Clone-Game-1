using System.Collections;
using TMPro;
using UnityEngine;

public class AnnouncementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI announcementText;
    [SerializeField] private float duration = 2f;

    public void Show(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        announcementText.text = message;
        announcementText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        announcementText.gameObject.SetActive(false);
    }
}
