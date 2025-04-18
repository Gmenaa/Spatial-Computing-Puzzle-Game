using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chemsceneloader : MonoBehaviour
{
    public void GoToScenechem()
    {
        SceneManager.LoadScene("chemcomb");
    }
}
