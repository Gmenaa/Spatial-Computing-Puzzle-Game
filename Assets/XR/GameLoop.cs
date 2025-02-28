using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.SceneManagement;
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
    private bool isSolved;      // boolean check whether puzzle has been solved (will be determined by puzzle script)

    [SerializeField]
    private bool isGameOver;    // boolean check whether game is over (causes: timer running out or some loss condition in the puzzle script being met)

    private bool isDoorLocked;  // boolean check whether door is locked (gets updated upon puzzle solved)

    [SerializeField]
    private bool restartGame;   // the game over menu will likely have a 'Restart Game' button, this gets sets to true if user clicks restart

    // handlers

    void HandleSolved()
    {
        // FIXME:
            // for first three rooms, will open door and then teleport player
            // for last room, the game ends and a 'You Win' screen appears?
        
        Debug.Log("Solved!");

        // FIXME:
            // will need information from the puzzle script whether it is the last room or not
            // right now will be hardcoding as "no", but this should be pulled from the puzzle script

        bool isLastRoom = false; 
        
        if (isLastRoom)
        {
            // FIXME:
                // handle winning (game ends and there's a 'you win!' screen)
        }
        else
        {
            isDoorLocked = false;

            // FIXME:
                // use the isDoorLocked check to update appearance of the door object(?)
                // implement the teleporting the player to the next scene
        }
    }

    // ! this was added by Gabriel to test the win condition
    public void TriggerWin()
    {
        isSolved = true;
    }

    void HandleGameOver()
    {
        // FIXME:
            // do something when loss condition met
            // will probably involve a menu appearing
            // that menu will probably have a restart button on it so you can restart the puzzle
        
        Debug.Log("Game Over!");

        // FIXME:
            // restartGame will need to be updated upon user interaction with the menuu

        if (restartGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // before the first frame
    void Start()
    {
        timer.InitialTimeRemaining = 180f;
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
        else if (timer.DidTimeRunOut)
        {
            // FIXME:
                // handle the situation that the timer has run out
            
            HandleGameOver();
        }
    }
}
