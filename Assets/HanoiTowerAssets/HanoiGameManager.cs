using UnityEngine;
using Oculus.Interaction;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction.HandGrab;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text winMessageText;
    
    private void Start()
    {
        winMessageText.gameObject.SetActive(false);
    }

    public void HandleWin()
    {
        winMessageText.gameObject.SetActive(true);
    }
}