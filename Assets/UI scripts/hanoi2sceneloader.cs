using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hanoi2sceneloader : MonoBehaviour
{
    public void GoToScenehanoi2()
    {
        SceneManager.LoadScene("HanoiTowers_2");
    }
}
