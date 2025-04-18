using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "BallMaze"; // scene name
    public float fadeDuration = 2f; // Time for fade out
    public CanvasGroup introCanvasGroup; // Canvas Group here

    void Start()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            introCanvasGroup.alpha = 1 - (time / fadeDuration);
            yield return null;
        }

        // Ensure fully invisible and disable interactions
        introCanvasGroup.alpha = 0f;
        introCanvasGroup.interactable = false;
        introCanvasGroup.blocksRaycasts = false;

        // Load the next scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
