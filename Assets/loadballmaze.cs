using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string BallMaze; // Assign the name of the scene to load in the Inspector

    public void LoadNewScene()
    {
        SceneManager.LoadScene(BallMaze);
    }
}