using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MusicAndFilterController : MonoBehaviour
{
    [Header("Timer Reference")]
    public Timer timer;  
 
    [Header("Audio Settings")]
    public AudioSource audioSource;   // GameObject that plays the music.
    public float finalVolume = 1f;    //  Target volume for the music.

    [Header("SFX Settings")]
    public AudioSource sfxAudioSource;      // AudioSource dedicated to SFX
    public AudioClip levelCompleteClip;     // The “level complete” sound effect
    public AudioClip gameOverClip;        // The “game over” sound effect
 
    [Header("Post Processing Volume")]
    public Volume volume;  // Volume that contains post processing effects.
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;

    // Track whether we are fading out/resetting effects
    private bool isResetting = false;

    void Start()
    {
        // Initialize audio to start at zero volume.
        if (audioSource != null)
        {
            audioSource.volume = 0f;
        }

        // Fetch the effects from the Volume Profile, if present.
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromaticAberration);
            volume.profile.TryGet(out filmGrain);

            // Initialize effect intensities to zero.
            if(vignette != null)
                vignette.intensity.value = 0f;
            if(chromaticAberration != null)
                chromaticAberration.intensity.value = 0f;
            if(filmGrain != null)
                filmGrain.intensity.value = 0f;
        }
    }

    void Update()
    {
        // If we're in the middle of resetting/fading out, skip the normal update logic.
        if (isResetting) 
            return;

        // Calculate progress from 0 (timer start) to 1 (time's up).
        float progress = (timer.InitialTimeRemaining - timer.TimeRemaining) / timer.InitialTimeRemaining;
        progress = Mathf.Clamp01(progress);

        // Update the music volume gradually (0 -> finalVolume).
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Lerp(0f, finalVolume, progress);
        }

        // Update screen filter effects (0 -> desired max).
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 0.5f, progress);
        }
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 0.33f, progress);
        }
        if (filmGrain != null)
        {
            filmGrain.intensity.value = Mathf.Lerp(0f, 1f, progress);
        }
    }

    /// <summary>
    /// Smoothly resets music volume and effects to 0.
    /// </summary>
    public void FadeBackToDefaults(float duration = 2f)
    {
        // Stop the normal update logic.
        isResetting = true;

        // Start a coroutine that fades everything to default over 'duration' seconds.
        StartCoroutine(FadeEffectsToDefault(duration));
    }

    private System.Collections.IEnumerator FadeEffectsToDefault(float duration)
    {
        // Capture current intensities/volume.
        float startVolume = (audioSource != null) ? audioSource.volume : 0f;
        float startVignette = (vignette != null) ? vignette.intensity.value : 0f;
        float startChromatic = (chromaticAberration != null) ? chromaticAberration.intensity.value : 0f;
        float startFilmGrain = (filmGrain != null) ? filmGrain.intensity.value : 0f;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Fade music volume -> 0
            if (audioSource != null)
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t);

            // Fade all effects -> 0
            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(startVignette, 0f, t);

            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(startChromatic, 0f, t);

            if (filmGrain != null)
                filmGrain.intensity.value = Mathf.Lerp(startFilmGrain, 0f, t);

            yield return null;
        }

        // Ensure final values are set to 0
        if (audioSource != null)
            audioSource.volume = 0f;
        if (vignette != null)
            vignette.intensity.value = 0f;
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 0f;
        if (filmGrain != null)
            filmGrain.intensity.value = 0f;
    }

    /// <summary>
    /// Plays the "level complete" sound effect (OneShot).
    /// </summary>
    public void PlayLevelCompleteSound()
    {
        if (sfxAudioSource != null && levelCompleteClip != null)
        {
            sfxAudioSource.PlayOneShot(levelCompleteClip);
        }
        else
        {
            Debug.LogWarning("Missing SFX AudioSource or Level Complete Clip!");
        }
    }

    /// <summary>
    /// Plays the "game over" sound effect (OneShot).
    /// </summary>
    public void PlayGameOverSound()
    {
        if (sfxAudioSource != null && gameOverClip != null)
        {
            sfxAudioSource.PlayOneShot(gameOverClip);
        }
    }
}
