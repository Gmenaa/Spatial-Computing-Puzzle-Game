using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Made by: Helen McKay | 2102556

// notes:
    // this is a general game loop for each scene
    // if we decide to implement damage from consequences (solving puzzles wrong), will need to add more to this code
    // not sure how to implement the title screen yet from a code standpoint??
    // all variables are private, some use properties (C#'s getters and setters)
    // variables that need to be accessed in Unity have [SerializeField]

// usage:
    // should be used in every puzzle scene
    // feel free to change the name of this file if a different name makes more sense
    // puzzles should have their own scripts, with variables that GameLoop can check to see if win/loss conditions have been met
    // I've been using the structure:
        // Game Loop > Canvas > Timer > Text (TMP)
            // with Text (TMP) getting the Time Text from Timer, and Game Loop getting Timer for its timer
public class GameLoop : MonoBehaviour
{
    // variables

    [SerializeField]
    private Timer timer;        // The Timer object
    [SerializeField]
    private LightToggleController lightToggleController;

    [SerializeField]
    private bool isSolved;      // boolean check whether puzzle has been solved (will be determined by puzzle script)

    [SerializeField]
    private bool isGameOver;    // boolean check whether game is over (causes: timer running out or some loss condition in the puzzle script being met)

    private bool isDoorLocked;  // boolean check whether door is locked (gets updated upon puzzle solved)
    private float initialTimerValue;

    [SerializeField]
    private bool restartGame;   // the game over menu will likely have a 'Restart Game' button, this gets sets to true if user clicks restart

    [SerializeField]
    private MusicAndFilterController musicAndFilterController;

    [SerializeField]
    private GameObject gameOverCanvas;
//for transition
    [SerializeField] 
    private float sceneTransitionDelay = 1.5f;
    [SerializeField]
    private Animator transitionAnimator;
    [SerializeField] 
    private string transitionTrigger = "FadeOut";


    // handlers

    void HandleSolved()
    {
        // FIXME:
            // for first three rooms, will open door and then teleport player
            // for last room, the game ends and a 'You Win' screen appears?
        
        Debug.Log("Solved!");

        // Optionally fade out music/effects over 2 seconds
        if (musicAndFilterController != null)
        {
            musicAndFilterController.PlayLevelCompleteSound();
            musicAndFilterController.FadeBackToDefaults(2f);
        }

        if (lightToggleController != null)
        {
            lightToggleController.ResetLightColor(1f);  
        }

        // FIXME:
            // will need information from the puzzle script whether it is the last room or not
            // right now will be hardcoding as "no", but this should be pulled from the puzzle script

        bool isLastRoom = false; 
        
        if (isLastRoom)
        
        {
            // FIXME:Transition
                // handle winning (game ends and there's a 'you win!' screen)
                StartCoroutine(LoadSceneWithTransition(8));

        }
        else
        {
            isDoorLocked = false;
            StartCoroutine(LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex + 1));

            // FIXME:  Transition
                // use the isDoorLocked check to update appearance of the door object(?)
                // implement the teleporting the player to the next scene
        }
    }
  //for transition  
    IEnumerator LoadSceneWithTransition(int sceneBuildIndex)
   
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        SceneManager.LoadScene(sceneBuildIndex);
    }


    public void TriggerWin()
    {
        isSolved = true;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
    }

    //restart button
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    void HandleGameOver()
    {
        // FIXME:
            // do something when loss condition met
            // will probably involve a menu appearing
            // that menu will probably have a restart button on it so you can restart the puzzle
        
        Debug.Log("Game Over!");

        if (musicAndFilterController != null)
        {
            musicAndFilterController.PlayGameOverSound();

            // Optionally fade out the music and effects, just like a "win"
            musicAndFilterController.FadeBackToDefaults(2f);
        }

        // FIXME:
            // restartGame will need to be updated upon user interaction with the menuu
            //ui integrated
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        if (restartGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // before the first frame
    void Start()
    {
        // set the timer to the correct time depending on index
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        string sceneName = SceneManager.GetActiveScene().name;
        
        switch (sceneIndex)
        {
            case 1: // ball maze
                initialTimerValue = 60f; 
                break;
            case 2: // chemical combination
                initialTimerValue = 120f; 
                break;
            case 3: // hanoi 1
                initialTimerValue = 180f; 
                break;
            case 4: // hanoi 2
                initialTimerValue = 300f; 
                break;
            case 5: // pipes
                initialTimerValue = 120f; 
                break;
            case 6: // mirrros
                initialTimerValue = 120f; 
                break;
            default:
                initialTimerValue = 180f; 
                break;
        }

    
        timer.InitialTimeRemaining = initialTimerValue;
        timer.ResetTimer();
        
        isSolved = false;
        isDoorLocked = true;
    }

    // every frame
    void Update()
    {
        if (timer.IsTimerRunning)
        {
            if (isGameOver)
            {
                // FIXME:
                    // to start with, this check will just be flipped manually in Unity
                    // will need to set isGameOver to true upon some loss condition in the puzzle being met

                timer.StopTheTimer();
                HandleGameOver();
            }
            
            if (isSolved)
            {
                // FIXME:
                    // to start with, this check will just be flipped manually in Unity
                    // will need to set isSolved to true upon some win condition in the puzzle being met

                timer.StopTheTimer();
                HandleSolved();
            }
        }
        else if (timer.DidTimeRunOut && !isGameOver)
        {
            // FIXME:
                // handle the situation that the timer has run out
            
            isGameOver = true;
            HandleGameOver();
        }

    }
}
