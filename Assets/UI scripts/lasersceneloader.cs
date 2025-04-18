using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lasersceneloader : MonoBehaviour
{
    public void GoToScenelaser()
    {
        SceneManager.LoadScene("LaserMirror");
    }
}
