using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by: Gabriel Mena : 2031820

// notes:
    // This script is attached to the exit trigger in the maze
    // When the ball enters the trigger, the win condition is met and the timer is paused

public class MazeExitTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the ball
        if (other.CompareTag("MazeBall"))
        {
            Debug.Log("Ball has exited the maze!");

            // Get the GameLoop instance from the scene
            GameLoop gameLoop = FindObjectOfType<GameLoop>();
            if (gameLoop != null)
            {
                // Trigger the win condition in GameLoop
                gameLoop.TriggerWin();
            }
            else
            {
                Debug.LogError("GameLoop not found in scene!");
            }
        }
    }
}

