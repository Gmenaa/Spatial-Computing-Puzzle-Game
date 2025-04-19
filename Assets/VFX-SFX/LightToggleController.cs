using UnityEngine;
using System.Collections;

public class LightToggleController : MonoBehaviour
{
    [Header("Timer Reference")]
    public Timer timer; 

    [Header("Light Settings")]
    public Light lightObject;             
    public Color startColor = Color.white; 
    public Color targetColor = Color.red;  

    [Header("Transition Settings")]
    public float transitionDuration = 3f;
    
    // Flag to freeze the automatic timer-based transition.
    private bool freezeTransition = false;

    void Start()
    {
        if (lightObject == null)
        {
            lightObject = GetComponent<Light>();
            if (lightObject == null)
            {
                Debug.LogError("LightToggleController: No Light component found on this GameObject.");
            }
        }

        if (lightObject != null)
        {
            lightObject.color = startColor;
        }
    }

    void Update()
    {
        if (freezeTransition)
            return;

        if (timer == null || lightObject == null)
            return;

        float threshold = timer.InitialTimeRemaining / 3f; // Color shift will start at 1/3 of the initial time remaining.

        // Start transitioning as soon as timer's remaining time reaches or drops below the threshold.
        if (timer.TimeRemaining <= threshold)
        {
            float elapsedSinceThreshold = threshold - timer.TimeRemaining;
            float progress = Mathf.Clamp01(elapsedSinceThreshold / transitionDuration);
            lightObject.color = Color.Lerp(startColor, targetColor, progress);
        }
        else
        {
            lightObject.color = startColor; // Before hitting the threshold, keep the light at its starting color.
        }
    }

    /// <summary>
    /// Public method to reset the light color back to the start color.
    /// When called, it freezes the timer-based updates and transitions the light back.
    /// </summary>
    /// <param name="duration">Duration over which the transition occurs.</param>
    public void ResetLightColor(float duration = 1f)
    {
        freezeTransition = true;
        StartCoroutine(ResetColorCoroutine(duration));
    }

    /// <summary>
    /// Coroutine that transitions the light color back to startColor over the specified duration.
    /// </summary>
    /// <param name="duration">Time in seconds for the transition.</param>
    private IEnumerator ResetColorCoroutine(float duration)
    {
        float elapsed = 0f;
        Color currentColor = lightObject.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            lightObject.color = Color.Lerp(currentColor, startColor, progress);
            yield return null;
        }

        lightObject.color = startColor;
    }
}
