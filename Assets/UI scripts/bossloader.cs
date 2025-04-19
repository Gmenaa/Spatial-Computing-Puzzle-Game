using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossloader : MonoBehaviour
{
    public void GoToSceneboss()
    {
        SceneManager.LoadScene("finalBoss");
    }
}
