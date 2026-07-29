using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class FlickerEffect : MonoBehaviour
{
    private Image targetImage;
    
    [Header("Flicker Settings")]
    [Tooltip("Minimum time the image is visible before flickering to black")]
    public float minVisibleTime = 0.1f;
    [Tooltip("Maximum time the image is visible before flickering to black")]
    public float maxVisibleTime = 0.5f;
    
    [Tooltip("Minimum time the image stays black")]
    public float minBlackTime = 0.05f;
    [Tooltip("Maximum time the image stays black")]
    public float maxBlackTime = 0.15f;
    
    private Color originalColor;
    
    void Start()
    {
        targetImage = GetComponent<Image>();
        originalColor = targetImage.color;
        StartCoroutine(FlickerRoutine());
    }
    
    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Wait for a random amount of time while visible
            yield return new WaitForSeconds(Random.Range(minVisibleTime, maxVisibleTime));
            
            // Turn black
            targetImage.color = Color.black;
            
            // Stay black for a random, brief moment (strobe effect)
            yield return new WaitForSeconds(Random.Range(minBlackTime, maxBlackTime));
            
            // Return to original color (which reveals the image again)
            targetImage.color = originalColor;
        }
    }
}
