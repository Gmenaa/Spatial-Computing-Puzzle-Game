using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenuloader : MonoBehaviour
{
    public void GoTomenu()
    {
        SceneManager.LoadScene("main menu");
    }
}
