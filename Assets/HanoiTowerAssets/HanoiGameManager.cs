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
    public GameObject[] rods;
    public int totalDisks = 4;

    void Start()
    {
        if (winMessageText != null)
            winMessageText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        foreach (GameObject rod in rods)
        {
            bool rodIsCorrect = true;
            for (int pos = 1; pos <= totalDisks; pos++)
            {
                string positionName = rod.name + "diskPosition" + pos;
                Transform posTransform = rod.transform.Find(positionName);
                if (posTransform == null)
                {
                    rodIsCorrect = false;
                    break;
                }
                if (posTransform.childCount == 0)
                {
                    rodIsCorrect = false;
                    break;
                }
                GameObject disk = posTransform.GetChild(0).gameObject;
                string expectedDiskName = "SquareHanoiDisk" + (totalDisks - pos + 1);
                if (disk.name != expectedDiskName)
                {
                    rodIsCorrect = false;
                    break;
                }
            }
            if (rodIsCorrect)
            {
                if (winMessageText != null)
                    winMessageText.gameObject.SetActive(true);
                return;
            }
        }
        if (winMessageText != null)
            winMessageText.gameObject.SetActive(false);
    }
}