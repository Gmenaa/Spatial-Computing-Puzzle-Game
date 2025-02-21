using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Made by: Helen McKay | 2102556

// notes:
    // all variables are private
    // variables that need to be accessed in Unity have [SerializeField]
    // if any of these variables later need to be accessed or updated elsewhere in the project, can use properties (C#'s getters and setters)
        // will only set those up when they're needed
    // made a function for handling timer reaching 0 (if it's not needed, can remove the function)

// usage:
    // attach this script to an object
    // connect a Text Mesh Pro (TMP) object to the TMP_Text field timeText
    // feel free to chage the value of INITIAL_TIME_REMAINING to whatever is needed for the scene
    // feel free to add [SerializeField] to more variables as desired

public class Timer: MonoBehaviour
{
    // consts

    [SerializeField]
    private float INITIAL_TIME_REMAINING = 180f;    // the value the timer starts at, serializable so can edit its value in Unity

    // variables
    
    private float rawTimeRemaining;                 // the time remaining, as a float
    private bool isTimerRunning;                    // boolean check of whether the timer is still running
    [SerializeField]
    private TMP_Text timeText;                      // Text Mesh Pro object that displays the time
    private bool didTimeRunOut;                     // boolean check of whether timer reached zero (meaning, player did not solve puzzle in time)

    // getFormattedTime
        // formats the time in minutes and seconds
        // receives time as a float
        // returns formatted time as a string
    private string getFormattedTime(float the_time)
    {
        float minutes = Mathf.FloorToInt(the_time / 60);
        float seconds = Mathf.FloorToInt(the_time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // handlers

    private void handleTimeRunOut()
    {
        // FIXME: handle time run out
    }

    // before first frame
        // initialize variables
    private void Start()
    {
        rawTimeRemaining = INITIAL_TIME_REMAINING;
        timeText.text = getFormattedTime(rawTimeRemaining);
        isTimerRunning = true;
        didTimeRunOut = false;
    }

    // every frame
    void Update()
    {
        if (isTimerRunning)
        {
            if (rawTimeRemaining > 0)
            {
                rawTimeRemaining -= Time.deltaTime;
                timeText.text = getFormattedTime(rawTimeRemaining);
            }
            else
            {
                rawTimeRemaining = 0;
                timeText.text = getFormattedTime(0);
                isTimerRunning = false;
                didTimeRunOut = true;
            }
        }
        else if (didTimeRunOut)
        {
            handleTimeRunOut();
        }
    }
}