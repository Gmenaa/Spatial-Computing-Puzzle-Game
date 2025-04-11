using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MusicAndFilterController : MonoBehaviour
{
    [Header("Timer Reference")]
    public Timer timer;  
 
    [Header("Audio Settings")]
    public AudioSource audioSource;   // GameObject that plays the music.
    public float finalVolume = 1f;    

    [Header("SFX Settings")]
    public AudioSource sfxAudioSource;      // AudioSource dedicated to SFX
    public AudioClip levelCompleteClip;     
    public AudioClip gameOverClip;         
 
    [Header("Post Processing Volume")]
    public Volume volume;  // Volume that contains post processing effects.
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
 
    [Header("Additional Effects Settings")]
    public float finalBloomIntensity = 1.5f;
    public float finalLensDistortionIntensity = -0.2f; // ? (-) 'barrel' effect
    public float finalSaturation = -20f;               
    
    private Bloom bloom;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;
 
    // Track whether we are fading out/resetting effects.
    private bool isResetting = false;
 
    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
        }
 
        // Fetch the effects from the Volume Profile
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromaticAberration);
            volume.profile.TryGet(out filmGrain);
 
            volume.profile.TryGet(out bloom);
            volume.profile.TryGet(out lensDistortion);
            volume.profile.TryGet(out colorAdjustments);
 
            // Initialize effect intensities to zero.
            if (vignette != null)
                vignette.intensity.value = 0f;
            if (chromaticAberration != null)
                chromaticAberration.intensity.value = 0f;
            if (filmGrain != null)
                filmGrain.intensity.value = 0f;
 
            if (bloom != null)
                bloom.intensity.value = 0f;
            if (lensDistortion != null)
                lensDistortion.intensity.value = 0f;
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = 0f;
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
            audioSource.volume = Mathf.Lerp(0f, finalVolume, progress);
            Debug.Log("Audio Volume: " + audioSource.volume);
 
        // Update base screen effects.
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0f, 1f, progress);
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, progress);
        if (filmGrain != null)
        {
            if (timer.TimeRemaining <= 60f)
            {
                // Calculate progress over the final minute.
                float filmGrainProgress = Mathf.Clamp01((60f - timer.TimeRemaining) / 60f);
                filmGrain.intensity.value = Mathf.Lerp(0f, 1f, filmGrainProgress);
            }
            else
            {
                filmGrain.intensity.value = 0f;
            }
        }
            
 
        // Update additional effects for tension.
        if (bloom != null)
            bloom.intensity.value = Mathf.Lerp(0f, finalBloomIntensity, progress);
        if (lensDistortion != null)
            lensDistortion.intensity.value = Mathf.Lerp(0f, finalLensDistortionIntensity, progress);
        if (colorAdjustments != null)
            colorAdjustments.saturation.value = Mathf.Lerp(0f, finalSaturation, progress);
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
        float startBloom = (bloom != null) ? bloom.intensity.value : 0f;
        float startLensDistortion = (lensDistortion != null) ? lensDistortion.intensity.value : 0f;
        float startSaturation = (colorAdjustments != null) ? colorAdjustments.saturation.value : 0f;
 
        float timeElapsed = 0f;
 
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
 
            // Fade music volume -> 0.
            if (audioSource != null)
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
 
            // Fade base effects -> 0.
            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(startVignette, 0f, t);
            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(startChromatic, 0f, t);
            if (filmGrain != null)
                filmGrain.intensity.value = Mathf.Lerp(startFilmGrain, 0f, t);
 
            // Fade additional effects -> 0.
            if (bloom != null)
                bloom.intensity.value = Mathf.Lerp(startBloom, 0f, t);
            if (lensDistortion != null)
                lensDistortion.intensity.value = Mathf.Lerp(startLensDistortion, 0f, t);
            if (colorAdjustments != null)
                colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, 0f, t);
 
            yield return null;
        }
 
        // Ensure final values are set to 0.
        if (audioSource != null)
            audioSource.volume = 0f;
        if (vignette != null)
            vignette.intensity.value = 0f;
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 0f;
        if (filmGrain != null)
            filmGrain.intensity.value = 0f;
        if (bloom != null)
            bloom.intensity.value = 0f;
        if (lensDistortion != null)
            lensDistortion.intensity.value = 0f;
        if (colorAdjustments != null)
            colorAdjustments.saturation.value = 0f;
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
