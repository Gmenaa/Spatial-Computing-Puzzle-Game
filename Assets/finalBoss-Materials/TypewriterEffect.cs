using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    public float charactersPerSecond = 100f;
    public TMP_Text textComponent;

    public AudioSource audioSource;  
    public AudioClip winClip;         

    private string fullText;
    private TMP_TextInfo textInfo;

    void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        textComponent.ForceMeshUpdate();
        textInfo = textComponent.textInfo;
        textComponent.maxVisibleCharacters = 0;
    }

    public void Play(string message)
    {
        StopAllCoroutines();
        fullText = message;
        textComponent.text = fullText;
        textComponent.ForceMeshUpdate();
        textInfo = textComponent.textInfo;
        textComponent.maxVisibleCharacters = 0;
        StartCoroutine(RevealCharacters());
    }

    // Coroutine to reveal characters one by one
    private IEnumerator RevealCharacters()
    {
        int totalChars = textInfo.characterCount;
        float delay = 1f / Mathf.Max(charactersPerSecond, 1f);

        for (int i = 1; i <= totalChars; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }

        // When the last character is visible, play win sound
        if (winClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(winClip);
        }
    }
}
