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
    public TMP_Text loseMessageText;
    
    private void Start()
    {
        winMessageText.gameObject.SetActive(false);
        loseMessageText.gameObject.SetActive(false);
    }

    public void HandleWin()
    {
        GameLoop gameLoop = FindObjectOfType<GameLoop>();
        if (gameLoop != null)
        {
            gameLoop.TriggerWin();
        }
        else
        {
            Debug.LogError("GameLoop not found in scene!");
        }
        winMessageText.gameObject.SetActive(true);
    }

    public void HandleLose()
    {
        loseMessageText.gameObject.SetActive(true);
    }
}