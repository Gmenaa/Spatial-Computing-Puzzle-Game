using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{
    public void GoToScene2()
    {
        SceneManager.LoadScene("BallMaze");
    }
}
