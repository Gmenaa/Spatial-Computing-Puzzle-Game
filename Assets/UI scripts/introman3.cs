using UnityEngine;
using UnityEngine.SceneManagement;

public class introman3 : MonoBehaviour
{
    public float introDuration = 3f; 

    void Start()
    {
        Invoke("LoadpipesScene", introDuration);
    }

    void LoadpipesScene()
    {
        SceneManager.LoadScene("pipes"); 
    }
}
