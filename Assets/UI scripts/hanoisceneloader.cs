using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hanoisceneloader : MonoBehaviour
{
    public void GoToScene4()
    {
        SceneManager.LoadScene("HanoiTowers");
    }
}
