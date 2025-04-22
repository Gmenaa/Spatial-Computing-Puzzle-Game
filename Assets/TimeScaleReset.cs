using UnityEngine;
using UnityEngine.SceneManagement;

// This script resets the time scale to 1 when the game starts or when the scene is loaded.
// Ensure that the game runs at normal speed after being paused or when starting a new scene.
public class TimeScaleReset : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1;
    }
}

